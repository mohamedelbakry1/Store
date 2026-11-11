using Store.Domain.Entities.Products;
using Store.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifications.Products
{
    internal class ProductsCountSpec : BaseSpecification<Product, int>
    {
        public ProductsCountSpec(ProductQueryParams queryParams) :
            base(P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value) &&
            (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value) &&
            (string.IsNullOrEmpty(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {

        }
    }
}
