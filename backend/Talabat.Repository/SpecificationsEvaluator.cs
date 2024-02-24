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
    public static class SpecificationsEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecifications<T> specifications)
        {
            var query= InputQuery;
            if(specifications.Cretiria is not null)
            {
                query = query.Where(specifications.Cretiria);
            }
            if(specifications.OrderBy is not null)
            {
                query= query.OrderBy(specifications.OrderBy);
            }
            if (specifications.OrderByDes is not null)
            {
                query = query.OrderBy(specifications.OrderByDes);
            }

            if (specifications.IsPaginationEnabled)
            {
                query = query.Skip(specifications.Skip).Take(specifications.Take);
            }
          //if(specifications.Search is not null)
          //  {
          //      query=query.Where()
          //  }
            query = specifications.Includes.Aggregate(query, (CurrentQuery, IncludeExpression)
                                 => CurrentQuery.Include(IncludeExpression));
            return query;
        }
    }
}
