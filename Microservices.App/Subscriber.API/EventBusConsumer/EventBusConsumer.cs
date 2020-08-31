using EventBus;
using EventBus.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using Subscriber.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Subscriber.API.EventBusConsumer
{
    public class EventBusConsumer
    {
        
        private readonly EventConsumer _eventBus;

        public EventBusConsumer(EventConsumer eventBus)
        {
            _eventBus = eventBus;
        }

        public void Consume()
        {
            _eventBus.Consume(EventBusConstants.ProductQueue, ReceivedEvent);          
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == EventBusConstants.ProductQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<Event<Product>>(message);

              
            }
        }

        public void Disconnect()
        {
            _eventBus.Dispose();
        }
    }

    public static class ApplicationBuilderExtentions
    {
        public static EventBusConsumer Listener { get; set; }

        public static IApplicationBuilder UseListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<EventBusConsumer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Consume();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}
