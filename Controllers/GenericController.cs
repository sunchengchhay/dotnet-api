using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Product;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        public GenericController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepository.GetByIdAsnyc(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostProductDto product)
        {
            var productEntity = new Product()
            {
                ProductName = product.ProductName,
                Price = product.Price
            };

            var createdProductResponse = await _productRepository.AddAsync(productEntity);
            return CreatedAtAction(nameof(GetById), new { id = createdProductResponse.ProductId }, createdProductResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PostProductDto product)
        {
            var productEntity = await _productRepository.GetByIdAsnyc(id);
            if (productEntity == null)
            {
                return NotFound();
            }

            productEntity.ProductName = product.ProductName;
            productEntity.Price = product.Price;

            await _productRepository.UpdateAsync(productEntity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsnyc(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteAsync(product);
            return NoContent();
        }

    }
}