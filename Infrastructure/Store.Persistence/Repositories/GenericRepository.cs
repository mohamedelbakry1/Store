using Microsoft.EntityFrameworkCore;
using Store.Domain.Contracts;
using Store.Domain.Entities;
using Store.Domain.Entities.Products;
using Store.Persistence.Data.Contexts;
using Store.Persistence.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey>(StoreDbContext dbContext) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        public void Add(TEntity entity) => dbContext.Set<TEntity>().Add(entity);

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> spec, CancellationToken ct = default)
        {
            return await SpecificationEvaluator.CreateQuery<TEntity, TKey>(dbContext.Set<TEntity>(), spec).AsNoTracking().CountAsync(ct);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(bool changeTracker = false, CancellationToken ct = default)
        {
            return changeTracker ? await dbContext.Set<TEntity>().ToListAsync() 
                : await dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> spec, bool changeTracker = false, CancellationToken ct = default)
        {
            return changeTracker ? await SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), spec).ToListAsync() 
                : await SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), spec).AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
        {
            return await dbContext.Set<TEntity>().FindAsync(id, ct);
        }

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.CreateQuery<TEntity, TKey>(dbContext.Set<TEntity>(), spec);
            return await query.AsNoTracking().FirstOrDefaultAsync(ct);
        }

        public void Remove(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => dbContext.Set<TEntity>().Update(entity);
    }
}
