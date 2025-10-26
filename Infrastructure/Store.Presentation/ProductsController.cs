using Microsoft.AspNetCore.Mvc;
using Store.Services.Abstractions;
using Store.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet] // GET: baseurl/api/products
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters parameters)
        {
            var result = await _serviceManager.ProductService.GetAllProductsAsync(parameters);
            if (result is null) return BadRequest(); // 400
            return Ok(result); // 200
        }

        [HttpGet("{id}")] // GET: baseurl/api/products/id
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest();

            var result = await _serviceManager.ProductService.GetProductByIdAsync(id.Value);
            if (result is null) return NotFound(); // 400
            return Ok(result); // 200
        }

        [HttpGet("brands")] // GET: baseurl/api/products/brands
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest(); // 400
            return Ok(result); // 200
        }

        [HttpGet("types")] // GET: baseurl/api/products/types
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await _serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest(); // 400
            return Ok(result); // 200
        }
    }
}
