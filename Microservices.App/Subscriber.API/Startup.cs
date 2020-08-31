using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Subscriber.API.EventBusConsumer;

namespace Subscriber.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //#region Configuration RabbitMQ

            //var busConfiguration = Configuration.GetSection(nameof(EventBusConfiguration)).Get<EventBusConfiguration>();
            //if (busConfiguration != null)
            //    services.AddEventBus(busConfiguration);
            //#endregion
            //services.AddSingleton<EventBusConsumer.EventBusConsumer>();
            //services.AddSingleton<EventConsumer>();
            //services.AddTransient<IEventBusConnection, RabbitMQConnection>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            //app.UseListener();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        } }
    }

