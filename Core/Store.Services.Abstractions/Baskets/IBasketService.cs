using Store.Shared.Dtos.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Basket
{
    public interface IBasketService
    {
        Task<BasketDto?> GetBasketAsync(string id);
        Task<BasketDto?> CreateBasketAsync(BasketDto dto, TimeSpan dustation);
        Task<bool> DeleteBasketAsync(string id);

    }
}
