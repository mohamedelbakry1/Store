using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Store.Domain.Contracts;
using Store.Persistence.Data.Contexts;
using Store.Persistence.DataSeeding;
using Store.Persistence.Identity.Data.Contexts;
using Store.Persistence.Identity.Entities;
using Store.Persistence.Identity.Services;
using Store.Persistence.Payments;
using Store.Persistence.Repositories;
using Store.Services.Abstractions.Auth;
using Store.Services.Abstractions.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<IdentityStoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            });

            services.AddSingleton<IPaymentGateway, StripePaymentGateway>();

            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog");
            services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>("Identity");
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();

            services.AddIdentityCore<AppUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<IdentityStoreDbContext>();

            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new InvalidOperationException("Jwt Settings is missing");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }
    }
}
