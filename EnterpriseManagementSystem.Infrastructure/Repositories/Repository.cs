using EnterpriseManagementSystem.Application.Common;
using EnterpriseManagementSystem.Application.Interfaces;
using EnterpriseManagementSystem.Domain.Common;
using EnterpriseManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EnterpriseManagementSystem.Infrastructure.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>  where TEntity : BaseEntity<TKey>
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, bool track = true, CancellationToken cancellationToken = default)
    {
        if (!track)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
        }
        return await _dbSet.FindAsync(new object[] { id! }, cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool track = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        if (!track)
        {
            query = query.AsNoTracking();
        }
        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool track = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(predicate);
        if (!track)
        {
            query = query.AsNoTracking();
        }
        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool track = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(predicate);
        if (!track)
        {
            query = query.AsNoTracking();
        }
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate == null)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }
        return await _dbSet.CountAsync(predicate, cancellationToken);
    }

    public virtual async Task<PagedList<TEntity>> GetFilteredPaginatedAsync(
    IEnumerable<Expression<Func<TEntity, bool>>>? predicates = null,
    int pageNumber = 1,
    int pageSize = 10,
    bool track = true,
    Expression<Func<TEntity, object>>? orderBy = null,
    bool ascending = true,
    params Expression<Func<TEntity, object>>[] includes)
    {
        orderBy ??= e => EF.Property<object>(e, "Id");

        IQueryable<TEntity> query = _dbSet;

        if (!track)
            query = query.AsNoTracking();

        if (predicates != null && predicates.Any())
        {
            foreach (var predicate in predicates)
            {
                query = query.Where(predicate);
            }
        }

        if (includes != null && includes.Length > 0)
            foreach (var include in includes)
                query = query.Include(include);

        
        query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<TEntity>(totalCount, pageNumber, pageSize, items);
    }



    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public virtual async Task RemoveByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, track: true, cancellationToken);
        if (entity != null)
        {
            Remove(entity);
        }
    }
}

