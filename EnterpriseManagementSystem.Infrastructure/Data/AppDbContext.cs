using EnterpriseManagementSystem.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace EnterpriseManagementSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    // DbSets
    public DbSet<Domain.Entities.Employee> Employees => Set<Domain.Entities.Employee>();
    public DbSet<Domain.Entities.Entity1> Entity1s => Set<Domain.Entities.Entity1>();
    public DbSet<Domain.Entities.Entity2> Entity2s => Set<Domain.Entities.Entity2>();
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (IsFullAuditableEntity(clrType))
            {
                modelBuilder.Entity(clrType)
                            .HasQueryFilter(GetIsDeletedFilter(clrType));
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    private static bool IsAuditableEntity(Type type)
    {
        if (type == null) return false;
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType)
            {
                var genericDef = type.GetGenericTypeDefinition();
                if (genericDef == typeof(AuditableEntity<>)
                    || genericDef == typeof(FullAuditableEntity<>))
                {
                    return true;
                }
            }
            type = type.BaseType;
        }
        return false;
    }

    private static bool IsFullAuditableEntity(Type type)
    {
        if (type == null) return false;
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType)
            {
                var genericDef = type.GetGenericTypeDefinition();
                if (genericDef == typeof(FullAuditableEntity<>))
                    return true;
            }
            type = type.BaseType;
        }
        return false;
    }

    private static LambdaExpression GetIsDeletedFilter(Type clrType)
    {
        var param = Expression.Parameter(clrType, "e");
        var prop = Expression.Property(param, nameof(FullAuditableEntity<object>.IsDeleted));
        var condition = Expression.Equal(prop, Expression.Constant(false));
        return Expression.Lambda(condition, param);
    }

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        HandleSoftDelete();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        HandleSoftDelete();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => IsAuditableEntity(e.Entity.GetType())
                        && (e.State == EntityState.Added || e.State == EntityState.Modified));

        var now = DateTime.UtcNow;
        var currentUser = GetCurrentUser();

        foreach (var entry in entries)
        {
            var type = entry.Entity.GetType();

            if (entry.State == EntityState.Added)
            {
                SetPropertyValue(entry.Entity, nameof(AuditableEntity<object>.CreatedAt), now);
                SetPropertyValue(entry.Entity, nameof(AuditableEntity<object>.CreatedBy), currentUser);
            }
            else
            {
                Entry(entry.Entity).Property(nameof(AuditableEntity<object>.CreatedAt)).IsModified = false;
                Entry(entry.Entity).Property(nameof(AuditableEntity<object>.CreatedBy)).IsModified = false;
            }

            SetPropertyValue(entry.Entity, nameof(AuditableEntity<object>.UpdatedAt), now);
            SetPropertyValue(entry.Entity, nameof(AuditableEntity<object>.UpdatedBy), currentUser);
        }
    }

    private void HandleSoftDelete()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => IsFullAuditableEntity(e.Entity.GetType())
                        && e.State == EntityState.Deleted);

        var now = DateTime.UtcNow;
        var currentUser = GetCurrentUser();

        foreach (var entry in entries)
        {
            SetPropertyValue(entry.Entity, nameof(FullAuditableEntity<object>.IsDeleted), true);
            SetPropertyValue(entry.Entity, nameof(FullAuditableEntity<object>.DeletedAt), now);
            SetPropertyValue(entry.Entity, nameof(FullAuditableEntity<object>.DeletedBy), currentUser);

            entry.State = EntityState.Modified;
        }
    }

    private static void SetPropertyValue(object entity, string propertyName, object? value)
    {
        var prop = entity.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (prop != null && prop.CanWrite)
            prop.SetValue(entity, value);
    }

    private string? GetCurrentUser()
    {
        // TODO: Replace with actual user service
        return null;
    }
}
