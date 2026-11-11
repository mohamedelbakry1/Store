using AutoMapper;
using Store.Domain.Entities.Baskets;
using Store.Shared.DTOS.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Mapping.Baskets
{
    internal class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, CustomerBasketDTO>().ReverseMap();
            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();
        }
    }
}
