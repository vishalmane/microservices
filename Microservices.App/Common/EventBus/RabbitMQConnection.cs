using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace EventBus
{
    public class RabbitMQConnection : IEventBusConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;

        public event EventHandler<EventArgs> Received;

        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            if (!IsConnected)
            {
                TryConnect();
            }
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                Thread.Sleep(2000);
                _connection = _connectionFactory.CreateConnection();
            }

            if (IsConnected)
            {
                Console.WriteLine($"RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events");
                return true;
            }
            else
            {
                Console.WriteLine("FATAL ERROR: RabbitMQ connections could not be created and opened");
                return false;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }
            return _connection.CreateModel();
        }

        public void Publish<T>(string queueName, Event<T> obj) where T : class
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(obj);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: queueName, mandatory: true, basicProperties: properties, body: body);
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Sent RabbitMQ");
                    //implement ack handle
                };
                channel.ConfirmSelect();
            }
        }

        public void Consume(string queueName, EventHandler<BasicDeliverEventArgs> received)
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            //Create event when something receive
            consumer.Received += received;

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }


        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
