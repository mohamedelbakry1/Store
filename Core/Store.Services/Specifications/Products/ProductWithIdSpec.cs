using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifications.Products
{
    internal class ProductWithIdSpec : BaseSpecification<Product, int>
    {
        public ProductWithIdSpec(HashSet<int> productIds) : base(p => productIds.Contains(p.Id))
        {

        }
    }
}
