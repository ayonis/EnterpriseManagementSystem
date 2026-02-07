using EnterpriseManagementSystem.Domain.Common;

namespace EnterpriseManagementSystem.Domain.Entities;


public class Employee : FullAuditableEntity<int>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public DateTime? HireDate { get; set; }
    
    public int? IdentityUserId { get; set; }
}

