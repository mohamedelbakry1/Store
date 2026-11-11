using AutoMapper;
using Store.Domain.Contracts;
using Store.Domain.Entities.Baskets;
using Store.Services.Abstractions.Basket;
using Store.Shared.Common;
using Store.Shared.DTOS.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Baskets
{
    internal class BasketService(IBasketRepository _basketRepository, IMapper _mapper) : IBasketService
    {
        public async Task<Result<CustomerBasketDTO>> CreateOrUpdateBasketAsync(CustomerBasketDTO basketDTO, TimeSpan? timeToLive = default, CancellationToken ct = default)
        {
            var basket = _mapper.Map<CustomerBasket>(basketDTO);
            var result = await _basketRepository.CreateOrUpdateBasketAsync(basket, timeToLive, ct);
            if (result == null)
                return Error.Failure("CreateOrUpdateBasket.Failure", "Failed to create or update the basket.");
            return _mapper.Map<CustomerBasketDTO>(result);
        }

        public async Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
            var result = await _basketRepository.DeleteBasketAsync(basketId, ct);
            return result ? result : Error.Failure("DeleteBasket.Failure", "Failed to delete the basket.");
        }

        public async Task<Result<CustomerBasketDTO>> GetBasketAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId, ct);
            if (basket == null)
                return Error.NotFound("Basket.NotFound", $"Basket with id {basketId} is not found");
            return _mapper.Map<CustomerBasketDTO>(basket);
        }
    }
}
