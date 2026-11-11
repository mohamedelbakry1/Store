using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Abstractions;
using Store.Services.Abstractions.Basket;
using Store.Shared.DTOS.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    public class BasketsController(IBasketService _basketService) : ApiBaseController
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerBasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerBasketDTO>> GetBasket(string id, CancellationToken ct)
        {
            var result = await _basketService.GetBasketAsync(id, ct);
            return ToActionResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdateBasket(CustomerBasketDTO basketDTO, CancellationToken ct)
        {
            var result = await _basketService.CreateOrUpdateBasketAsync(basketDTO, ct: ct);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken ct)
        {
            var result = await _basketService.DeleteBasketAsync(id, ct);
            return ToActionResult(result);
        }
    }
}
