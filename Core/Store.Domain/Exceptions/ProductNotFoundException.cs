using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Exceptions
{
    public class ProductNotFoundException(int id) : 
        NotFoundException($"Product with id {id} was Not Found !!")
    {
    }
}
