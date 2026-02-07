namespace AuthenticationModule.Application.DTOs;

public class RegisterResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
    public string? Message { get; set; }
}

