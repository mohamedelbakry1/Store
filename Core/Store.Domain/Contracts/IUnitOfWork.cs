using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Contracts
{
    public interface IUnitOfWork
    {
        // Generate Repository

        IGenericRepository<Tkey, TEntity> GetRepository<Tkey, TEntity>() where TEntity : BaseEntity<Tkey>;

        // Save Changes
        Task<int> SaveChangesAsync();
    }
}
