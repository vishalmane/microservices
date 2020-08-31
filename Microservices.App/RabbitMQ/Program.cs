using EventBus;
using RabbitMQ.Client;
using System;

namespace RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                UserName= "guest",
                Password= "guest",
            };

            RabbitMQConnection connection = new RabbitMQConnection(factory);
            connection.Publish<TestMessage>("MyTestQueue", new Event<TestMessage>(new TestMessage { Address = "test Address", Name = "testUser" }));


        }
    }
    public class TestMessage
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
