using Store.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Payments
{
    public interface IPaymentGateway
    {
        Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default);
        Task<PaymentIntentResult> UpdatePaymentIntentAsync(decimal amount, string paymentIntentId, CancellationToken ct = default);
    }
}
