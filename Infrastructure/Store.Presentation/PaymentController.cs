using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Store.Services.Abstractions.Payments;
using Store.Shared.Common;
using Store.Shared.DTOS.Baskets;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    public class PaymentController(IPaymentService _paymentService, IOptions<PaymentGatewaySettings> options) : ApiBaseController
    {
        private readonly PaymentGatewaySettings _options = options.Value;

        [Authorize]
        [HttpPost("{basketId}")]
        [ProducesResponseType(typeof(CustomerBasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(string basketId, CancellationToken ct)
        {
            return ToActionResult(await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId, ct));
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            string endpointSecret = _options.WebhookSecret;
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);

                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    var paymentIntentSucceeded = stripeEvent.Data.Object as PaymentIntent;
                    if(paymentIntentSucceeded != null)
                        await _paymentService.PaymentSucceeded(paymentIntentSucceeded.Id);   
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    var paymentIntentFailed= stripeEvent.Data.Object as PaymentMethod;
                    if (paymentIntentFailed != null)
                        await _paymentService.PaymentFailed(paymentIntentFailed.Id);
                }
                else
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}", stripeEvent.Type);
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
