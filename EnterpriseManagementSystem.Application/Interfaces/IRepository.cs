using EnterpriseManagementSystem.Application.Common;
using EnterpriseManagementSystem.Domain.Common;
using System.Linq.Expressions;

namespace EnterpriseManagementSystem.Application.Interfaces;

public interface IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, bool track = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(bool track = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool track = true, CancellationToken cancellationToken = default);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool track = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<PagedList<TEntity>> GetFilteredPaginatedAsync(
    IEnumerable<Expression<Func<TEntity, bool>>>? predicates = null,
    int pageNumber = 1,
    int pageSize = 10,
    bool track = true,
    Expression<Func<TEntity, object>>? orderBy = null,
    bool ascending = true,
    params Expression<Func<TEntity, object>>[] includes);
    
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    Task RemoveByIdAsync(TKey id, CancellationToken cancellationToken = default);
}

