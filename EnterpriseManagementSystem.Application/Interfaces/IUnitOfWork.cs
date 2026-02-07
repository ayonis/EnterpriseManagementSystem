using EnterpriseManagementSystem.Domain.Common;

namespace EnterpriseManagementSystem.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
    int Save();
}

