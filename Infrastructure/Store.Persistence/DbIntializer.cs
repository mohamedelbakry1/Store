using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Contracts;
using Store.Domain.Entities.Identity;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Products;
using Store.Persistence.Data.Contexts;
using Store.Persistence.Identity.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Persistence
{
    public class DbIntializer(
        StoreDbContext _context,
        IdentityStoreDbContext _identityContext,
        UserManager<AppUser> _userManager,
        RoleManager<IdentityRole> _roleManager
        ) : IDbIntializer
    {
        public async Task IntializeAsync()
        {
            // Create Db
            // Update Db

            if(_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _context.Database.MigrateAsync();
            }

            // Data Seeding

            if (!_context.ProductBrands.Any())
            {
                // Brands
                // 1. Read All Data From Json File 'brands.json'
                var brandsdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Persistence\Data\DataSeeding\brands.json");

                // 2. Convert the JsonString to List<ProductBrand>
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsdata);

                // 3. Add List to Db
                if (brands is not null && brands.Count > 0)
                {
                    await _context.ProductBrands.AddRangeAsync(brands);
                }
            }


            // Types

            if (!_context.ProductTypes.Any())
            {
                // Types
                // 1. Read All Data From Json File 'types.json'
                var typesdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Persistence\Data\DataSeeding\types.json");

                // 2. Convert the JsonString to List<ProductType>
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesdata);

                // 3. Add List to Db
                if (types is not null && types.Count > 0)
                {
                    await _context.ProductTypes.AddRangeAsync(types);
                }
            }

            // Product

            if (!_context.Products.Any())
            {
                // Products
                // 1. Read All Data From Json File 'products.json'
                var productsdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Persistence\Data\DataSeeding\products.json");

                // 2. Convert the JsonString to List<Product>
                var products = JsonSerializer.Deserialize<List<Product>>(productsdata);

                // 3. Add List to Db
                if (products is not null && products.Count > 0)
                {
                    await _context.Products.AddRangeAsync(products);
                }
            }

            if (!_context.DeliveryMethods.Any())
            {
                // DeliveryMethod
                // 1. Read All Data From Json File 'delivery.json'
                var deliveryData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Persistence\Data\DataSeeding\delivery.json");

                // 2. Convert the JsonString to List<DeliveryMethods>
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                // 3. Add List to Db
                if (deliveryMethods is not null && deliveryMethods.Count > 0)
                {
                    await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                }
            }

            await _context.SaveChangesAsync();

        }

        public async Task IntializeIdentityAsync()
        {
            if(_identityContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _identityContext.Database.MigrateAsync();
            }

            if (!_identityContext.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            }

            if (!_identityContext.Users.Any())
            {
                var superAdmin = new AppUser()
                {
                    UserName = "SuperAdmin",
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "01552013062"
                };

                var admin = new AppUser()
                {
                    UserName = "Admin",
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01015594223"
                };

                await _userManager.CreateAsync(superAdmin, "P@ssw0rd");
                await _userManager.CreateAsync(admin, "P@ssw0rd");

                await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                await _userManager.AddToRoleAsync(superAdmin, "Admin");
            }
        }
    }
}
