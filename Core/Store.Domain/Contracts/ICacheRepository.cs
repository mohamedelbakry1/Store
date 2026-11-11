using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Contracts
{
    public interface ICacheRepository
    {
        Task<string?> GetAsync(string key, CancellationToken ct =default);
        Task SetAsync(string key, string value, TimeSpan? timeToLive = default, CancellationToken ct = default);
    }
}
