using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Models;
using ProductApi.Core.Services;
using SharedModels;
using SharedModels.Product;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IConverter<Product, ProductDto> _productConverter;

        public ProductsController(IProductService service, IConverter<Product,ProductDto> converter)
        {
            _service = service;
            _productConverter = converter;
        }

        // GET products
        [HttpGet]
        public IActionResult Get()
        {
            var productDtoList = _service.GetAll().Select(product => _productConverter.Convert(product)).ToList();

            return Ok(productDtoList);
        }

        // GET products/5
        [HttpGet("{id}", Name="GetProduct")]
        public IActionResult Get(int id)
        {
            var item = _service.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            var productDto = _productConverter.Convert(item);
            return Ok(productDto);
        }

        // POST products
        [HttpPost]
        public IActionResult Post([FromBody]ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest();
            }

            var product = _productConverter.Convert(productDto);
            var newProduct = _service.Add(product);

            return CreatedAtRoute("GetProduct", new { id = newProduct.Id },
                    _productConverter.Convert(newProduct));
        }

        // PUT products/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ProductDto productDto)
        {
            if (productDto == null || productDto.Id != id)
            {
                return BadRequest();
            }

            var modifiedProduct = _service.Get(id);

            if (modifiedProduct == null)
            {
                return NotFound();
            }

            modifiedProduct.Name = productDto.Name;
            modifiedProduct.Price = productDto.Price;
            modifiedProduct.ItemsInStock = productDto.ItemsInStock;
            modifiedProduct.ItemsReserved = productDto.ItemsReserved;

            _service.Edit(modifiedProduct);
            return NoContent();
        }

        // DELETE products/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _service.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            _service.Remove(product);
            return NoContent();
        }
    }
}
