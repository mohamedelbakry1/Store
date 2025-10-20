using Store.Domain.Contracts;
using Store.Domain.Entities;
using Store.Persistence.Data.Contexts;
using Store.Persistence.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence
{
    public class UnitOfWork(StoreDbContext _context) : IUnitOfWork
    {
        //private readonly Dictionary<string,object> _repositories = new Dictionary<string,object>();
        private readonly ConcurrentDictionary<string, object> _repositories = new ConcurrentDictionary<string, object>();
        //public IGenericRepository<Tkey, TEntity> GetRepository<Tkey, TEntity>() where TEntity : BaseEntity<Tkey>
        //{
        //    var type = typeof(TEntity).Name;
        //    if (!_repositories.ContainsKey(type))
        //    {
        //        var repository = new GenericRepository<Tkey, TEntity>(_context);
        //        _repositories.Add(type, repository);
        //    }
        //    return (IGenericRepository<Tkey,TEntity>) _repositories[type];
        //}

        public IGenericRepository<Tkey, TEntity> GetRepository<Tkey, TEntity>() where TEntity : BaseEntity<Tkey>
        {
            return (IGenericRepository<Tkey,TEntity>) _repositories.GetOrAdd(typeof(TEntity).Name,new GenericRepository<Tkey,TEntity>(_context));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
