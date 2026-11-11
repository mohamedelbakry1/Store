using AutoMapper;
using Store.Domain.Contracts;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Products;
using Store.Domain.Exceptions.BadRequest;
using Store.Domain.Exceptions.NotFound;
using Store.Services.Abstractions.Orders;
using Store.Services.Specifications.Orders;
using Store.Services.Specifications.Products;
using Store.Shared.Common;
using Store.Shared.DTOS.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Orders
{
    internal class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository) : IOrderService
    {
        public async Task<Result<OrderReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email, CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId, ct);

            if (basket == null)
                return Error.NotFound("Basket.NotFound", $"Basket with id {orderDTO.BasketId} not found.");
            if (basket.BasketItems.Count == 0)
                return Error.Validation("Basket.Empty", $"Basket with id {orderDTO.BasketId} is empty.");


            var orderExists = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new PaymentIntentSpec(basket.PaymentIntentId!));

            if (orderExists != null)
                _unitOfWork.GetRepository<Order, Guid>().Remove(orderExists);

            var orderItems = new List<OrderItem>(basket.BasketItems.Count);
            var productIds = basket.BasketItems.Select(p => p.Id).ToHashSet();
            var products = (await _unitOfWork.GetRepository<Product, int>().GetAllAsync(new ProductWithIdSpec(productIds))).ToDictionary(x => x.Id);

            foreach (var item in basket.BasketItems)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Error.NotFound("Product.NotFound", $"Product with id {item.Id} not found.");

                orderItems.Add(new OrderItem
                {
                    Price = product.Price,
                    Quantity = item.Quantity,
                    Product = new()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl
                    }
                });
            }

            var shippingAddress = _mapper.Map<OrderAddress>(orderDTO.ShippingAddress);

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if (deliveryMethod is null)
                return Error.NotFound("DeliveryMethod.NotFound", $"DeliveryMethod with id {orderDTO.DeliveryMethodId} is not found");

            var subTotal = orderItems.Sum(o => o.Price * o.Quantity);
            var total = deliveryMethod.Cost + subTotal;

            var order = new Order(email, shippingAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);

            _unitOfWork.GetRepository<Order, Guid>().Add(order);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            if (result == 0)
                return Error.Failure("CreateOrder.Failure", "Can`t Create Order Please Try again");
            else
            {
                await _basketRepository.DeleteBasketAsync(orderDTO.BasketId, ct);
                return _mapper.Map<OrderReturnDTO>(order);
            }
        }

        public async Task<Result<IReadOnlyList<DeliveryMethodsDTO>>> GetAllDeliveryMethods(CancellationToken ct = default)
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            if (!deliveryMethods.Any())
                return Error.NotFound("DeliveryMethods.NotFound", "There is no Delivery Methods Available");
            return Result<IReadOnlyList<DeliveryMethodsDTO>>.Ok(_mapper.Map<IReadOnlyList<DeliveryMethodsDTO>>(deliveryMethods));
        }

        public async Task<Result<IReadOnlyList<OrderReturnDTO>>> GetAllOrdersForUserAsync(string email, CancellationToken ct = default)
        {
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrdersWithIdAndEmailSpec(email));
            if (!orders.Any())
                return Error.NotFound("Orders.NotFound", "There No Orders For User");
            return Result<IReadOnlyList<OrderReturnDTO>>.Ok(_mapper.Map<IReadOnlyList<OrderReturnDTO>>(orders));
        }

        public async Task<Result<OrderReturnDTO>> GetOrderByIdAndEmailAsync(Guid id, string email, CancellationToken ct = default)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrdersWithIdAndEmailSpec(id, email));
            if (order is null)
                return Error.NotFound("Order.NotFound", $"Order with id {id} is not found");
            return _mapper.Map<OrderReturnDTO>(order);
        }
    }
}
