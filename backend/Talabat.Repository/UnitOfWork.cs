using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly ConnectifyContext _context;

        public UnitOfWork(ConnectifyContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.Contains(type))
            {
                var Repository = new GenericRepository<TEntity>(_context);
                _repositories.Add(type, Repository);
            }
            return _repositories[type] as IGenericRepository<TEntity>;
          
        }
    }
}
