using Store.Shared;
using Store.Shared.Common;
using Store.Shared.DTOS.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Products
{
    public interface IProductService
    {
        Task<Result<PaginatedResult<ProductDTO>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default);
        Task<Result<IReadOnlyList<BrandDTO>>> GetAllBrandsAsync(CancellationToken ct = default);
        Task<Result<IReadOnlyList<TypeDTO>>> GetAllTypesAsync(CancellationToken ct = default);
        Task<Result<ProductDTO?>> GetProductByIdAsync(int id, CancellationToken ct = default);
    }
}
