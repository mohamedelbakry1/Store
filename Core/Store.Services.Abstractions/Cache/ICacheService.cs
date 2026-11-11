using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Cache
{
    public interface ICacheService
    {
        Task<string?> GetDataAsync(string key, CancellationToken ct = default);
        Task SetDataAsync(string key, object value, TimeSpan? timeToLive = default, CancellationToken ct = default);
    }
}
