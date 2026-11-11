using AutoMapper;
using Store.Domain.Entities.Orders;
using Store.Shared.DTOS.Identity;
using Store.Shared.DTOS.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Mapping.Orders
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTO, OrderAddress>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ForMember(x => x.ProductId, src => src.MapFrom(x => x.Product.ProductId))
                                                .ForMember(x => x.ProductName, src => src.MapFrom(x => x.Product.ProductName))
                                                .ForMember(x => x.PictureUrl, src => src.MapFrom<OrderPictureUrlResolver>());

            CreateMap<Order, OrderReturnDTO>().ForMember(x => x.DeliveryMethod, src => src.MapFrom(x => x.DeliveryMethod.ShortName))
                                              .ForMember(x => x.DeliveryMethodCost, src => src.MapFrom(x => x.DeliveryMethod.Cost));

            CreateMap<DeliveryMethod, DeliveryMethodsDTO>();
        }
    }
}
