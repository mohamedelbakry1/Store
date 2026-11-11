using Store.Domain.Entities.Products;
using Store.Shared.Common;
using Store.Shared.DTOS.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifications.Products
{
    internal class ProductsWithTypeAndBrandSpec : BaseSpecification<Product, int>
    {
        public ProductsWithTypeAndBrandSpec(ProductQueryParams queryParams) :
            base(P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value) &&
            (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value) &&
            (string.IsNullOrEmpty(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {
            AddIncludes(p => p.Brand);
            AddIncludes(p => p.Type);

            switch (queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public ProductsWithTypeAndBrandSpec(int id) : base(p => p.Id == id)
        {
            AddIncludes(p => p.Brand);
            AddIncludes(p => p.Type);
        }
    }
}
