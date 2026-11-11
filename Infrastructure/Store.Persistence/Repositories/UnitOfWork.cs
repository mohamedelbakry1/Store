using Microsoft.EntityFrameworkCore;
using Store.Domain.Contracts;
using Store.Domain.Entities;
using Store.Persistence.Data.Contexts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Repositories
{
    public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<Type, object> _repositories = [];
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            return (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity),
                   _ => new GenericRepository<TEntity, TKey>(dbContext)
                    );
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return dbContext.SaveChangesAsync(ct);
        }
    }
}
