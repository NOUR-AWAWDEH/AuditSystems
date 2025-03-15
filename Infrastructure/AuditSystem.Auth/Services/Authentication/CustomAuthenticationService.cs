using AuditSystem.Auth.Authentication;
using AuditSystem.Auth.Dtos;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Authentication;

public class CustomAuthenticationService : ICustomAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public CustomAuthenticationService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null || string.IsNullOrEmpty(user.UserName))
            return null;

        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);
        if (!result.Succeeded)
            return null;

        var jwtTokenTask = Task.Run(() => _tokenService.GenerateJwtToken(user));
        var refreshTokenTask = Task.Run(() => _tokenService.GenerateRefreshToken());

        var jwtToken = await jwtTokenTask;
        var refreshToken = await refreshTokenTask;

        _ = _tokenService.SaveRefreshTokenAsync(user, refreshToken);

        return new LoginResponseDto
        {
            JwtToken = jwtToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<bool> SignOutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
