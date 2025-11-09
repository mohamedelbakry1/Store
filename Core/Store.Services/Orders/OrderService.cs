using AutoMapper;
using Store.Domain.Contracts;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Products;
using Store.Domain.Exceptions.BadRequest;
using Store.Domain.Exceptions.NotFound;
using Store.Services.Abstractions.Orders;
using Store.Services.Specifications.Orders;
using Store.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Orders
{
    public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string userEmail)
        {
            // 1. Get Order Address

            var orderAddress = _mapper.Map<OrderAddress>(request.ShipAddress);

            // 2. Get Delivery Method By Id

            var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAsync(request.DeliveryMethodId);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundException(request.DeliveryMethodId);


            // 3. Get Order Items
            // 3.1. Get Basket By Id
            var basket = await _basketRepository.GetBasketAsync(request.BasketId);
            if(basket is null) throw new BasketNotFoundException(request.BasketId);

            // 3.2. Convert Every Basket Item to Order Item

            var orderItems = new List<OrderItem>();

            foreach(var item in basket.Items)
            {
                // Check Price
                // Get Product From Db
                var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(item.Id);
                if(product is null) throw new ProductNotFoundException(item.Id);

                if(product.Price != item.Price) item.Price = product.Price;


                var productInOrderItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);
                var orderItem = new OrderItem(productInOrderItem, item.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            // 4. Calculate SubTotal

            var subTotal = orderItems.Sum(OI => OI.Price * OI.Quantity);


            // Create Order
            var order = new Order(userEmail, orderAddress, deliveryMethod, orderItems, subTotal);

            // Add Order in Database
            await _unitOfWork.GetRepository<Guid, Order>().AddAsync(order);
            var count = await _unitOfWork.SaveChangesAsync();
            if (count <= 0) throw new CreateOrderBadRequestException();

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethodAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliveryMethods);
        }

        public async Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid id, string UserEmail)
        {
            var spec = new OrderSpecification(id, UserEmail);
            var order = await _unitOfWork.GetRepository<Guid, Order>().GetAsync(spec);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrderForSpecificUserAsync(string UserEmail)
        {
            var spec = new OrderSpecification(UserEmail);
            var order = await _unitOfWork.GetRepository<Guid, Order>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<OrderResponse>>(order);
        }
    }
}
