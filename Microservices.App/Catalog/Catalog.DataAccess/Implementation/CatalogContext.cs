using Catalog.DataAccess.Implementation;
using Catalog.Model;
using Catalog.Model.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.DataAccess.Interfaces
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(ICatalogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Products = database.GetCollection<Product>(settings.CollectionName);
            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
