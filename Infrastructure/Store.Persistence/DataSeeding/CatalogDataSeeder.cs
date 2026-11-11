using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Domain.Contracts;
using Store.Domain.Entities;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Products;
using Store.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Persistence.DataSeeding
{
    internal class CatalogDataSeeder(StoreDbContext dbContext, ILogger<CatalogDataSeeder> logger) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);
                if (pendingMigrations.Any())
                    await dbContext.Database.MigrateAsync(ct);

                var seedRoot = Path.Combine(AppContext.BaseDirectory, "Data", "Files");

                await LoadDataFromJsonAsync<ProductBrand, int>(seedRoot, "brands.json", ct);
                await LoadDataFromJsonAsync<ProductType, int>(seedRoot, "types.json", ct);
                await LoadDataFromJsonAsync<Product, int>(seedRoot, "products.json", ct);
                await LoadDataFromJsonAsync<DeliveryMethod, int>(seedRoot, "delivery.json", ct);

                var result = await dbContext.SaveChangesAsync(ct);
                if (result > 0)
                    logger.LogInformation($"{result} rows added");
                else
                    logger.LogInformation("Database already seeded");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Data Seed Failed!");
            }
        }

        private async Task LoadDataFromJsonAsync<TEntity, TKey>(string roorPath, string fileName, CancellationToken ct = default) where TEntity : BaseEntity<TKey>
        {
            if (await dbContext.Set<TEntity>().AnyAsync(ct))
            {
                logger.LogInformation("Table has already seeded");
                return;
            }

            var filePath = Path.Combine(roorPath, fileName);
            if (!File.Exists(filePath))
            {
                logger.LogWarning($"File {fileName} is not found!");
                return;
            }

            using var fileStream = File.OpenRead(filePath);

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            var items = await JsonSerializer.DeserializeAsync<List<TEntity>>(fileStream, options, ct);
            if (items?.Any() ?? false)
                dbContext.Set<TEntity>().AddRange(items);
        }
    }
}
