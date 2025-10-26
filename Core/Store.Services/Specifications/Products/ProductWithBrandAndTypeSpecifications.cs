using Store.Domain.Entities.Products;
using Store.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifications.Products
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecifications<int,Product>
    {
        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }
        
        public ProductWithBrandAndTypeSpecifications(ProductQueryParameters parameters) : base
            (
                P => 
                (!parameters.BrandId.HasValue || P.BrandId == parameters.BrandId)
                &&
                (!parameters.TypeId.HasValue || P.TypeId == parameters.TypeId)
                &&
                (string.IsNullOrEmpty(parameters.Search) || P.Name.ToLower().Contains(parameters.Search.ToLower()))
            )
        {
            // pageIndex : 3
            // pageSize : 5
            // Skip : (pageIndex - 1) * pageSize
            // Take : 5

            ApplyPagination(parameters.PageSize, parameters.PageIndex);
            ApplySorting(parameters.Sort);
            ApplyIncludes();
        }

        private void ApplyIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }
        private void ApplySorting(string? sort)
        {
            // priceasc
            // pricedesc
            // nameasc
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }
        }
    }
}
