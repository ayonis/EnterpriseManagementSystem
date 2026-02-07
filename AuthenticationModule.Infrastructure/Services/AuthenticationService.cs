using AuthenticationModule.Application.DTOs;
using AuthenticationModule.Application.Interfaces;
using AuthenticationModule.Domain.Entities;
using AuthenticationModule.Domain.ValueObjects;
using AuthenticationModule.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthenticationModule.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly RoleManager<AppRole> _roleManager;

    public AuthenticationService(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Password != request.ConfirmPassword)
            throw new ArgumentException("Password and confirmation password do not match");

        // Create domain entity
        var email = Email.Create(request.Email);
        var user = User.Create(email, request.UserName, request.FirstName, request.LastName);

        // Map domain entity to infrastructure entity (AppUser for Identity)
        var appUser = new AppUser
        {
            UserName = user.UserName,
            Email = user.Email.Value,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };

        var result = await _userManager.CreateAsync(appUser, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"User registration failed: {errors}");
        }


        return new RegisterResponseDto
        {
            UserId = appUser.Id,
            Email = appUser.Email!,
            UserName = appUser.UserName!,
            EmailConfirmed = appUser.EmailConfirmed,
            Message = "User registered successfully"
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var appUser = await _userManager.FindByEmailAsync(request.Email);
        if (appUser == null || !appUser.IsActive)
            throw new UnauthorizedAccessException("Invalid email or password");

        var isValidPassword = await _userManager.CheckPasswordAsync(appUser, request.Password);
        if (!isValidPassword)
            throw new UnauthorizedAccessException("Invalid email or password");

        var roles = await _userManager.GetRolesAsync(appUser);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            new Claim(ClaimTypes.Name, appUser.UserName!),
            new Claim(ClaimTypes.Email, appUser.Email!),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = _tokenService.GenerateToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Store refresh token
        appUser.RefreshToken = refreshToken;
        appUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(appUser);

        return new LoginResponseDto
        {
            UserId = appUser.Id,
            Email = appUser.Email!,
            UserName = appUser.UserName!,
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            Roles = roles.ToList()
        };
    }

    public async Task LogoutAsync(string token, CancellationToken cancellationToken = default)
    {
        // In a production system, you might want to blacklist the token
        // For now, we'll just clear the refresh token
        var principal = _tokenService.ValidateToken(token);
        if (principal != null)
        {
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = null;
                    await _userManager.UpdateAsync(user);
                }
            }
        }
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        var principal = _tokenService.ValidateToken(request.Token);
        if (principal == null)
            throw new UnauthorizedAccessException("Invalid token");

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            throw new UnauthorizedAccessException("Invalid token");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid refresh token");

        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var newToken = _tokenService.GenerateToken(claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new LoginResponseDto
        {
            UserId = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            Token = newToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            Roles = roles.ToList()
        };
    }
}

