using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Core
{
    public interface IUnitOfWork:IAsyncDisposable 
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();

    }
}
