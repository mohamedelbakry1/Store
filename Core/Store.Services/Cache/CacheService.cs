using Store.Domain.Contracts;
using Store.Services.Abstractions.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Services.Cache
{
    internal class CacheService(ICacheRepository _cacheRepository) : ICacheService
    {
        public async Task<string?> GetDataAsync(string key, CancellationToken ct = default)
        {
            return await _cacheRepository.GetAsync(key, ct);
        }

        public async Task SetDataAsync(string key, object value, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            var redisValue = JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await _cacheRepository.SetAsync(key, redisValue, timeToLive, ct);
        }
    }
}
