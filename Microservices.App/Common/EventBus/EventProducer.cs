using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus
{
    public class EventProducer
    {
        private readonly IEventBusConnection _connection;

        public EventProducer(IEventBusConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public void Publish<T>(string queueName, Event<T> obj) where T : class
        {
            if (!_connection.IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }
            _connection.Publish<T>(queueName, obj);           
        }
    }

    public class EventConsumer
    {
        private readonly IEventBusConnection _connection;

        public EventConsumer(IEventBusConnection connection)
        {
            _connection = connection;
        }

        public void Consume(string queueName, EventHandler<BasicDeliverEventArgs> received)
        {
            _connection.Consume(queueName, received);
        }
        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
