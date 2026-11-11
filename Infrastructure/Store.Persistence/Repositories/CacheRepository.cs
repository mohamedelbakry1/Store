using StackExchange.Redis;
using Store.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Persistence.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<string?> GetAsync(string key, CancellationToken ct = default)
        {
            var value = await _database.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task SetAsync(string key, string value, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            await _database.StringSetAsync(key, value, timeToLive ?? TimeSpan.FromDays(2));
        }
    }
}
