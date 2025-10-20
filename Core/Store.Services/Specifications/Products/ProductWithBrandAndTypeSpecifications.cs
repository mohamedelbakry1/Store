using Store.Domain.Entities.Products;
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

        public ProductWithBrandAndTypeSpecifications() : base(null)
        {
            ApplyIncludes();
        }

        private void ApplyIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }
    }
}
