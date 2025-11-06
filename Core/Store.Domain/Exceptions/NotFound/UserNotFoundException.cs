using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Exceptions.NotFound
{
    public class UserNotFoundException(string email) : NotFoundException($"User with Email {email} was Not Found !!")
    {

    }
}
