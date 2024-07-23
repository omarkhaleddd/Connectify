using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Cretiria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDes { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public BaseSpecifications()
        {
           // Includes = new List<Expression<Func<T, object>>>();
        }
        public BaseSpecifications(Expression<Func<T,bool>> criteriaExpression)
        {
            Cretiria = criteriaExpression;
          //  Includes = new List<Expression<Func<T, object>>>();
        }

        public void AddOrderby(Expression<Func<T, object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;

        }
        public void AddOrderbyDesc(Expression<Func<T, object>> OrderByDescExpression)
        {
            OrderByDes = OrderByDescExpression;

        }
        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
