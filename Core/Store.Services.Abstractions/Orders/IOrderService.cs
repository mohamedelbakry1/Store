using Store.Shared.Common;
using Store.Shared.DTOS.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Orders
{
    public interface IOrderService
    {
        Task<Result<OrderReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<OrderReturnDTO>>> GetAllOrdersForUserAsync(string email, CancellationToken ct = default);
        Task<Result<OrderReturnDTO>> GetOrderByIdAndEmailAsync(Guid id, string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<DeliveryMethodsDTO>>> GetAllDeliveryMethods(CancellationToken ct = default);
    }
}
