using E_Commerce.API.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using Store.Persistence;
using Store.Persistence.Identity.Services;
using Store.Services;
using Store.Services.Mapping.Products;
using Store.Shared.Common;
using Store.Shared.ErrorModels;
using Store.Web.Middlewares;
using System.Threading.Tasks;

namespace Store.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection("UrlSettings"));
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.Configure<PaymentGatewaySettings>(builder.Configuration.GetSection("StripeSettings"));
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await app.SeedAndMigrateDataAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<GlobalErrorHandlingMiddlewares>();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Files")),
                RequestPath = "/Files"
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
