using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus
{
    public class EventBusConfiguration
    {
        public string BusType { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public static class EventBusRegistration
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, EventBusConfiguration Configuration)
        {

            if (Configuration.BusType.ToLower() == "rabbitmq")
            {

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration.HostName
                };

                if (!string.IsNullOrEmpty(Configuration.UserName))
                {
                    factory.UserName = Configuration.UserName;
                }

                if (!string.IsNullOrEmpty(Configuration.Password))
                {
                    factory.Password = Configuration.Password;
                }
                factory.VirtualHost = "/";
                //factory.Protocol = Protocols.FromEnvironment();
                //factory.HostName = "192.168.0.12";
                factory.Port = 5672;
                IConnection conn = factory.CreateConnection();
                var c = new RabbitMQConnection(factory);
                services.AddSingleton<IEventBusConnection>(sp =>
                 {
                     var factory = new ConnectionFactory()
                     {
                         HostName = Configuration.HostName
                     };

                     if (!string.IsNullOrEmpty(Configuration.UserName))
                     {
                         factory.UserName = Configuration.UserName;
                     }

                     if (!string.IsNullOrEmpty(Configuration.Password))
                     {
                         factory.Password = Configuration.Password;
                     }

                     return new RabbitMQConnection(factory);

                 });
            }
            return services;
        }
             
    }
}
