using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Abstractions;
using Store.Services.Abstractions.Orders;
using Store.Shared.DTOS.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    public class OrderController(IOrderService orderService) : ApiBaseController
    {
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(OrderReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderReturnDTO>> CreateOrder(OrderDTO orderDTO, CancellationToken ct)
        {
            return ToActionResult(await orderService.CreateOrderAsync(orderDTO, GetEmailFromToken(), ct));
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<OrderReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<OrderReturnDTO>>> GetAllOrders(CancellationToken ct)
        {
            return ToActionResult(await orderService.GetAllOrdersForUserAsync(GetEmailFromToken(), ct));
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(OrderReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderReturnDTO>> GetOrderByIdAndEmail(Guid id, CancellationToken ct)
        {
            return ToActionResult(await orderService.GetOrderByIdAndEmailAsync(id, GetEmailFromToken(), ct));
        }

        [HttpGet("deliveryMethods")]
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethodsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodsDTO>>> GetAllDeliveryMethods(CancellationToken ct)
        {
            return ToActionResult(await orderService.GetAllDeliveryMethods(ct));
        }
    }
}
