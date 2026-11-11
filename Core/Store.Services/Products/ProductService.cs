using AutoMapper;
using Store.Domain.Contracts;
using Store.Domain.Entities.Products;
using Store.Domain.Exceptions;
using Store.Domain.Exceptions.NotFound;
using Store.Services.Abstractions.Products;
using Store.Services.Specifications;
using Store.Services.Specifications.Products;
using Store.Shared;
using Store.Shared.Common;
using Store.Shared.DTOS.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Products
{
    internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<Result<IReadOnlyList<BrandDTO>>> GetAllBrandsAsync(CancellationToken ct = default)
        {

            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(false, ct);
            return Result<IReadOnlyList<BrandDTO>>.Ok(mapper.Map<IReadOnlyList<BrandDTO>>(brands));
        }

        public async Task<Result<PaginatedResult<ProductDTO>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default)
        {
            var spec = new ProductsWithTypeAndBrandSpec(queryParams);
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec, false, ct);
            var data = mapper.Map<IReadOnlyList<ProductDTO>>(products);

            var countSpec = new ProductsCountSpec(queryParams);
            var countOfAllProducts = await unitOfWork.GetRepository<Product, int>().CountAsync(countSpec, ct);

            return Result<PaginatedResult<ProductDTO>>.Ok(new PaginatedResult<ProductDTO>(queryParams.PageSize, queryParams.PageIndex, countOfAllProducts, data));
        }

        public async Task<Result<IReadOnlyList<TypeDTO>>> GetAllTypesAsync(CancellationToken ct = default)
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync(false, ct);
            return Result<IReadOnlyList<TypeDTO>>.Ok(mapper.Map<IReadOnlyList<TypeDTO>>(types));
        }

        public async Task<Result<ProductDTO?>> GetProductByIdAsync(int id, CancellationToken ct = default)
        {
            var spec = new ProductsWithTypeAndBrandSpec(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec, ct);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"Product with id {id} not found");
            return mapper.Map<ProductDTO>(product);
        }
    }
}
