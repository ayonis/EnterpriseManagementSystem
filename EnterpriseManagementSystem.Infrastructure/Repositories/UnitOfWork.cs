using EnterpriseManagementSystem.Application.Interfaces;
using EnterpriseManagementSystem.Domain.Common;
using EnterpriseManagementSystem.Infrastructure.Data;
using System.Collections.Concurrent;

namespace EnterpriseManagementSystem.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly ConcurrentDictionary<string, object> _repositories;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        _repositories = new ConcurrentDictionary<string, object>();
    }

    public IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
    {
        var key = $"{typeof(TEntity).FullName}_{typeof(TKey).FullName}";
        
        return (IRepository<TEntity, TKey>)_repositories.GetOrAdd(key, _ =>
        {
            var repositoryType = typeof(Repository<,>).MakeGenericType(typeof(TEntity), typeof(TKey));
            return Activator.CreateInstance(repositoryType, _context)!;
        });
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public int Save()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}

