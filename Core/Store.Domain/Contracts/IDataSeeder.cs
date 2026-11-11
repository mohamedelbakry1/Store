using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Contracts
{
    public interface IDataSeeder
    {
        Task SeedDataAsync(CancellationToken ct = default);
    }
}
