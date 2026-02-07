using Microsoft.AspNetCore.Identity;

namespace AuthenticationModule.Infrastructure.Entities;

/// <summary>
/// Identity User entity for EF Core - maps to auth.AspNetUsers
/// </summary>
public class AppUser : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}

