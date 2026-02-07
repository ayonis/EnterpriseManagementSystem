namespace EnterpriseManagementSystem.Domain.Common;

public abstract class FullAuditableEntity<TKey> : AuditableEntity<TKey>
{
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}
