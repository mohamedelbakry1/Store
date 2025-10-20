using AutoMapper;
using Store.Domain.Contracts;
using Store.Domain.Entities.Products;
using Store.Services.Abstractions.Products;
using Store.Services.Specifications;
using Store.Services.Specifications.Products;
using Store.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Products
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            //var spec = new BaseSpecifications<int, Product>(null);
            //spec.Includes.Add(P => P.Brand);
            //spec.Includes.Add(P => P.Type);

            var spec = new ProductWithBrandAndTypeSpecifications();

            var products = await _unitOfWork.GetRepository<int,Product>().GetAllAsync(spec);
            var result = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return result;
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications();

            var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(spec);
            var result = _mapper.Map<ProductResponse>(product);
            return result;
        }
        public async Task<IEnumerable<BrandTypeResponse>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<int, ProductBrand>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandTypeResponse>>(brands);
            return result;
        }

        public async Task<IEnumerable<BrandTypeResponse>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<int, ProductType>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandTypeResponse>>(types);
            return result;
        }


    }
}
