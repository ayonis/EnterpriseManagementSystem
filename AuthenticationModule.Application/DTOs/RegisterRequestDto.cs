namespace AuthenticationModule.Application.DTOs;

public class RegisterRequestDto
{
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

