using Microsoft.EntityFrameworkCore;
using Store.Domain.Contracts;
using Store.Domain.Entities.Products;
using Store.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Persistence
{
    public class DbIntializer(StoreDbContext _context) : IDbIntializer
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

            await _context.SaveChangesAsync();

        }
    }
}
