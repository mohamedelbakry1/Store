using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Presentation.Attributes;
using Store.Services.Abstractions.Products;
using Store.Shared.Common;
using Store.Shared.DTOS.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    [Authorize]
    public class ProductsController(IProductService productService) : ApiBaseController
    {
        [HttpGet]
        [RedisCache]
        [ProducesResponseType(typeof(PaginatedResult<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams, CancellationToken ct)
        {
            var result = await productService.GetAllProductsAsync(queryParams, ct);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO?>> GetProductById(int id, CancellationToken ct)
        {
            var result = await productService.GetProductByIdAsync(id, ct);
            return ToActionResult(result);
        }

        [HttpGet("types")]
        [ProducesResponseType(typeof(TypeDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<TypeDTO>>> GetAllTypes(CancellationToken ct)
        {
            var result = await productService.GetAllTypesAsync(ct);
            return ToActionResult(result);
        }

        [HttpGet("brands")]
        [ProducesResponseType(typeof(BrandDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<BrandDTO>>> GetAllBrands(CancellationToken ct)
        {
            var result = await productService.GetAllBrandsAsync(ct);
            return ToActionResult(result);
        }
    }
}
