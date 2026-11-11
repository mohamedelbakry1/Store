using AutoMapper;
using Store.Domain.Entities.Products;
using Store.Shared.DTOS.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Mapping.Products
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductType, TypeDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(p => p.ProductBrand, opt => opt.MapFrom(b => b.Brand.Name))
                .ForMember(p => p.ProductType, opt => opt.MapFrom(t => t.Type.Name))
                .ForMember(p => p.PictureUrl, opt => opt.MapFrom<PictureUrlResolver>());
        }
    }
    
}
