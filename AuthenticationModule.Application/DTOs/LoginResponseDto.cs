namespace AuthenticationModule.Application.DTOs;

public class LoginResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public List<string> Roles { get; set; } = new();
}

