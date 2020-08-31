using Catalog.DataAccess;
using Catalog.Repositories.Implementation;
using Catalog.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Repositories
{
    public static class ServicesConfiguration
    {
        public static void ConfigureRepositoryServices(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.ConfigureDataAccessServices();
        }
    }
}
