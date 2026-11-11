using Microsoft.Extensions.Options;
using Store.Services.Abstractions.Payments;
using Store.Shared.Common;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Payments
{
    internal class StripePaymentGateway : IPaymentGateway
    {
        private readonly PaymentIntentService _paymentIntentService = new();
        public StripePaymentGateway(IOptions<PaymentGatewaySettings> options)
        {
            StripeConfiguration.ApiKey = options.Value.SecretKey;
        }
        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)amount,
                Currency = currency,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                }
            };
            var paymentIntent = await _paymentIntentService.CreateAsync(options, cancellationToken: ct);
            return new PaymentIntentResult(paymentIntent.Id, paymentIntent.ClientSecret);
        }

        public async Task<PaymentIntentResult> UpdatePaymentIntentAsync(decimal amount, string paymentIntentId, CancellationToken ct = default)
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)amount
            };
            var paymentIntent = await _paymentIntentService.UpdateAsync(paymentIntentId, options, cancellationToken: ct);
            return new PaymentIntentResult(paymentIntent.Id, paymentIntent.ClientSecret);
        }
    }
}
