using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Contracts
{
    public interface ISpecifications<TKey,TEntity> where TEntity : BaseEntity<TKey>
    {
        public List<Expression<Func<TEntity,object>>> Includes { get; set; }
        public Expression<Func<TEntity,bool>> Criteria { get; set; }
    }
}
