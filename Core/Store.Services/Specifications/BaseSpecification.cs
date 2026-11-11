using Store.Domain.Contracts;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifications
{
    internal abstract class BaseSpecification<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool isPaginated { get; private set; }

        protected BaseSpecification(Expression<Func<TEntity, bool>> condition)
        {
            Criteria = condition;
        }

        protected void AddIncludes(Expression<Func<TEntity, object>> include)
        {
            IncludeExpressions.Add(include);
        }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
        }

        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            isPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }
    }
}
