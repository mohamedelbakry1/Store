using AutoMapper;
using Microsoft.Extensions.Options;
using Store.Domain.Contracts;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Products;
using Store.Services.Abstractions.Payments;
using Store.Services.Specifications.Orders;
using Store.Services.Specifications.Products;
using Store.Shared.Common;
using Store.Shared.DTOS.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Payments
{
    internal class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IOptions<PaymentGatewaySettings> _options;
        private readonly IMapper _mapper;

        public PaymentService(
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IPaymentGateway paymentGateway,
        IOptions<PaymentGatewaySettings> options,
        IMapper mapper
        )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentGateway = paymentGateway;
            _options = options;
            _mapper = mapper;
        }
        public async Task<Result<CustomerBasketDTO>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId, ct);
            if (basket == null)
                return Error.NotFound("Basket.NotFound", $"Basket with id {basketId} is not found");
            if (basket.BasketItems.Count == 0)
                return Error.Validation("Basket.Empty", $"Can`t Create Order with empty basket!!");


            if (basket.DeliveryMethodId == null)
                return Error.Validation("DeliveryMethod.Validation", "Delivery Method is Required");
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value, ct);
            if (deliveryMethod == null)
                return Error.NotFound("DeliveryMethod.NotFound", $"Delivery Method with id {basket.DeliveryMethodId} is not found");
            basket.ShippingPrice = deliveryMethod.Cost;

            var productsIds = basket.BasketItems.Select(i => i.Id).ToHashSet();
            var products = (await _unitOfWork.GetRepository<Product, int>().GetAllAsync(new ProductWithIdSpec(productsIds), false, ct)).ToDictionary(p => p.Id);

            foreach (var item in basket.BasketItems)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Error.NotFound("Product.NotFound", $"Product with id {item.Id} is not found");
                item.Price = product.Price;
            }

            var subTotal = basket.BasketItems.Sum(i => i.Price * i.Quantity);
            var totalAmount = (subTotal + deliveryMethod.Cost) * 100m;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var result = await _paymentGateway.CreatePaymentIntentAsync(totalAmount, _options.Value.DefaultCurrency, ct);
                basket.PaymentIntentId = result.PaymentIntentId;
                basket.ClientSecret = result.ClientSecret;
            }
            else
            {
                var result = await _paymentGateway.UpdatePaymentIntentAsync(totalAmount, basket.PaymentIntentId, ct);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket, ct: ct);

            return _mapper.Map<CustomerBasketDTO>(basket);
        }

        public async Task PaymentFailed(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new PaymentIntentSpec(paymentIntentId));
            if (order == null)
                return;
            order.Status = OrderStatus.PaymentFailed;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task PaymentSucceeded(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new PaymentIntentSpec(paymentIntentId));
            if (order == null)
                return;
            order.Status = OrderStatus.PaymentSuccess;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
