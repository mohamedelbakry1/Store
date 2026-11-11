using AutoMapper;
using Microsoft.Extensions.Options;
using Store.Domain.Entities.Orders;
using Store.Services.Mapping.Products;
using Store.Shared.DTOS.Orders;

namespace Store.Services.Mapping.Orders
{
    internal class OrderPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly UrlSettings _urlSettings;
        public OrderPictureUrlResolver(IOptions<UrlSettings> options)
        {
            _urlSettings = options.Value;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            var baseUrl = _urlSettings.BaseUrl.TrimEnd('/');
            var path = source.Product.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{path}";
        }
    }
}