using Catalog.DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.DataAccess
{
    public static class ServicesConfiguration
    {
        public static void ConfigureDataAccessServices(this IServiceCollection services)
        {
            services.AddTransient<ICatalogContext, CatalogContext>();
        }
    }
}
