using Microsoft.Extensions.DependencyInjection;
using Store.Services.Abstractions.Auth;
using Store.Services.Abstractions.Basket;
using Store.Services.Abstractions.Cache;
using Store.Services.Abstractions.Orders;
using Store.Services.Abstractions.Payments;
using Store.Services.Abstractions.Products;
using Store.Services.Auth;
using Store.Services.Baskets;
using Store.Services.Cache;
using Store.Services.Orders;
using Store.Services.Payments;
using Store.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services
{
    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(c => { }, typeof(ApplicationServicesRegisteration).Assembly);
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }
    }
}
