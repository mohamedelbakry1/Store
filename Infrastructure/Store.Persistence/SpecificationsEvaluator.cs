using Microsoft.EntityFrameworkCore;
using Store.Domain.Contracts;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence
{
    public static class SpecificationsEvaluator
    {
        // _context.Products.Include(P => P.Brand).Include(P => P.Type).FirstOrDefaultAsync(P => P.Id == key as int?) as TEntity
        public static IQueryable<TEntity> GetQuery<TKey,TEntity>(IQueryable<TEntity> inputQuery, ISpecifications<TKey,TEntity> spec) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery; // _context.Products
            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria); // _context.Products.Where(P => P.id == 12)
            }

            if(spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }else if(spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagination)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }


            // _context.Products.Where(P => P.id == 12)
            // _context.Products.Where(P => P.id == 12).Include(P => P.Brand).Include(P => P.Type)
            query = spec.Includes.Aggregate(query, (query, includeExpression) => query.Include(includeExpression));

            return query;
        }
    }
}
