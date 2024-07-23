using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ConnectifyContext _dbcontext;

        public GenericRepository(ConnectifyContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> specifications)
        {
            return await ApplySpec(specifications).ToListAsync();
        }
        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> specifications)
        {
            return await ApplySpec(specifications).FirstOrDefaultAsync();
        }
        private IQueryable<T> ApplySpec(ISpecifications<T> specifications)
        {
            return  SpecificationsEvaluator<T>.GetQuery(_dbcontext.Set<T>(), specifications);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> specifications)
        {
            return await ApplySpec(specifications).CountAsync();
        }

        public async Task Add(T item)
        {
            await _dbcontext.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
            _dbcontext.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
            _dbcontext.Set<T>().Update(item);
        }

        public void SaveChanges()
        {
            _dbcontext.SaveChanges();
        }
    }
}
