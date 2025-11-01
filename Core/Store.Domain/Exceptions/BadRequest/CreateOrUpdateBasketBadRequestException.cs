using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Exceptions.BadRequest
{
    public class CreateOrUpdateBasketBadRequestException() : 
        BadRequestException("Invalid Operation when Create or Update Basket !!")
    {
    }
}
