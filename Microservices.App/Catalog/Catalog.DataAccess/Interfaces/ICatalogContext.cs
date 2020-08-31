using Catalog.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.DataAccess.Interfaces
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
