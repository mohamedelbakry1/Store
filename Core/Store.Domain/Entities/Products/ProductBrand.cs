﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Products
{
    public class ProductBrand : BaseEntity<int>
    {
        public string Name { get; set; }
    }
}
