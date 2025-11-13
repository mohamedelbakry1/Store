using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Domain.Contracts;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Products;
using Store.Domain.Exceptions.NotFound;
using Store.Services.Abstractions.Payments;
using Store.Shared.Dtos.Basket;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Domain.Entities.Products.Product;

namespace Store.Services.Payments
{
    public class PaymentService(IBasketRepository _basketRepository, IUnitOfWork _unitOfWork, IConfiguration configuration, IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto?> CreatePaymentIntentAsync(string basketId)
        {
            // Calculate Amount = SubTotal + Delivery Method Cost

            // Get Basket by Id
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if(basket is null) throw new BasketNotFoundException(basketId);

            // Check Product and its Price
            foreach(var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(item.Id);
                if(product is null) throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }

            // Calculate SubTotal
            var subTotal = basket.Items.Sum(I => I.Price * I.Quantity);

            // Get Delivery Method by Id

            if (!basket.DeliveryMethodId.HasValue) throw new DeliveryMethodNotFoundException(-1);

            var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
            if(deliveryMethod is null) throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingCost = deliveryMethod.Price;

            var amount = subTotal + deliveryMethod.Price;

            // Send Amount to Stripe
            StripeConfiguration.ApiKey = configuration["StripeOptions:SecretKey"];

            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if(basket.PaymentIntentId is null)
            {
                // Create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)amount * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card"}
                };

                paymentIntent =  await paymentIntentService.CreateAsync(options);
            }
            else
            {
                // Update
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)amount * 100,
                };

                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntentId,options);
            }

            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

            basket = await _basketRepository.CreateBasketAsync(basket, TimeSpan.FromDays(1));
            return _mapper.Map<BasketDto>(basket);
        }
    }
}
