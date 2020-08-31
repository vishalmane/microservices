using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.Model;
using Catalog.Repositories.Interfaces;
using EventBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;
        //private readonly EventProducer _eventBus;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository;
            _logger = logger;
           // _eventBus = eventBus;
        }
        //public CatalogController(IProductRepository repository, ILogger<CatalogController> logger, EventProducer eventBus)
        //{
        //    _repository = repository;
        //    _logger = logger;
        //    _eventBus = eventBus;
        //}

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _repository.GetProduct(id);

            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, hasn't been found in database.");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{category}")]
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var product = await _repository.GetProductByCategory(category);
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.Create(product);

           // _eventBus.Publish<Product>("MyProduct", new Event<Product>(product));
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product value)
        {
            return Ok(await _repository.Update(value));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _repository.Delete(id));
        }

    }
}