using Store.Shared.Common;
using Store.Shared.DTOS.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Basket
{
    public interface IBasketService
    {
        Task<Result<CustomerBasketDTO>> GetBasketAsync(string basketId, CancellationToken ct = default);
        Task<Result<CustomerBasketDTO>> CreateOrUpdateBasketAsync(CustomerBasketDTO basketDTO, TimeSpan? timeToLive = default, CancellationToken ct = default);
        Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default);

    }
}
