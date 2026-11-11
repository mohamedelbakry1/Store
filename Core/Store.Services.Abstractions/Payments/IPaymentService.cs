
using Store.Shared.Common;
using Store.Shared.DTOS.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Payments
{
    public interface IPaymentService
    {
        Task<Result<CustomerBasketDTO>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default);
        Task PaymentSucceeded(string paymentIntentId);
        Task PaymentFailed(string paymentIntentId);
    }
}
