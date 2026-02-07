using Microsoft.AspNetCore.Identity;

namespace AuthenticationModule.Infrastructure.Entities;

/// <summary>
/// Identity Role entity for EF Core - maps to auth.AspNetRoles
/// </summary>
public class AppRole : IdentityRole<int>
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

