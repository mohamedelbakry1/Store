using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Contracts
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(bool changeTracker = false, CancellationToken ct = default);
        Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity,TKey> spec,bool changeTracker = false, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(TKey key, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(ISpecifications<TEntity,TKey> spec, CancellationToken ct = default);
        Task<int> CountAsync(ISpecifications<TEntity,TKey> spec, CancellationToken ct = default);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
