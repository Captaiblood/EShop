using Catalog.API.Dto.Property;
using Catalog.API.Entities;
using Catalog.API.Repository.Contract;
using Catalog.API.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {

        private readonly IProductService _service;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductService service, ILogger<CatalogController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _service.GetProductsAsync();
            //var products = await _repository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product.Data == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }
            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _service.GetProductByCategoryAsync(category);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]        
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto product)
        {
            if(product == null) { return BadRequest(ModelState); }
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var _newProduct =  await _service.CreateProductAsync(product);

            if (_newProduct.Success == false && _newProduct.Message == "Exist")
            {
                ModelState.AddModelError("", "Product Exist");
                return StatusCode(409, ModelState);
            }

            if (_newProduct.Success == false && _newProduct.Data == null)
            {
                ModelState.AddModelError("", $"Some thing went wrong when adding Product {product}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetProduct", new { id = _newProduct.Data.Id }, _newProduct);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto product)
        {
            if (product == null )
            {
                return BadRequest(ModelState);
            }


            var _updateProduct = await _service.UpdatePrpductAsync(product);

            if (_updateProduct.Success == false && _updateProduct.Message == "NotFound")
            {
                ModelState.AddModelError("", "Property Not found");
                return StatusCode(404, ModelState);
            }

            if (_updateProduct.Success == false && _updateProduct.Data == null)
            {
                ModelState.AddModelError("", $"Some thing went wrong when updating Property {product}");
                return StatusCode(500, ModelState);
            }

           


            return Ok(_updateProduct);
            //return Ok(await _service.UpdatePrpductAsync(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteProductById(string id)
        {

            var _deleteProperty = await _service.DeleteProductAsync(id);


            if (_deleteProperty.Success == false && _deleteProperty.Message == "NotFound")
            {
                ModelState.AddModelError("", "Product Not found");
                return StatusCode(404, ModelState);
            }

            if (_deleteProperty.Success == false && _deleteProperty.Data == null)
            {
                ModelState.AddModelError("", $"Some thing went wrong when deleting Product");
                return StatusCode(500, ModelState);
            }

            

            return Ok(_deleteProperty);

            //return Ok(await _repository.DeleteProduct(id));
        }


    }
}
