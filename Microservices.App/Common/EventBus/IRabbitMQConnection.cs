using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace EventBus
{
    public interface IEventBusConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        //ICommunicationModel CreateModel();
        void Publish<T>(string queueName, Event<T> obj) where T : class;
        void Consume(string queueName, EventHandler<BasicDeliverEventArgs> received);

    }
    //
    // Summary:
    //     Contains all the information about a message delivered from an AMQP broker within
    //     the Basic content-class.
    public class MessageReceivedEventArgs<T> : EventArgs
    {        
        public T Message { get; set; }
        public string ConsumerTag { get; set; }
        public ulong DeliveryTag { get; set; }
        public string Exchange { get; set; }
        public bool Redelivered { get; set; }
        public string RoutingKey { get; set; }
    }
    public interface ICommunicationModel
    { }
}
