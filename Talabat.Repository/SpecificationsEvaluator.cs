using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> Spec)
        {
            var query = inputQuery;  //query = _dbContext.Set<TEntity>()
            if (Spec.Criteria != null)
            {
                query = query.Where(Spec.Criteria);
                //query = _dbContext.Set<TEntity>().Where(x => x.Id == 1);
            }

            if(Spec.OrderBy is not null)
            {
                query = query.OrderBy(Spec.OrderBy);
            }
            else if (Spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(Spec.OrderByDescending);
            }

            if (Spec.IsPaginationEnabled)
            {
                query = query.Skip(Spec.Skip).Take(Spec.Take);
            }

            query = Spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            //_dbContext.Set<TEntity>().Where(x => x.Id == 1).OrderBy(x => x.name).Include(x => x.Brand).Include(x => x.Category)

            //string[] Names = { "Mohamed", "Alaa", "Eldeen" };
            //string message = "Hello";
            //message = Names.Aggregate(message, (current, next) => current + " " + next);
            return query;
        }
    }
}
