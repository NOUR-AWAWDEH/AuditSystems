using AuditSystem.Auth.Authentication;
using AuditSystem.Auth.Dtos;
using AuditSystem.DataAccess.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuditSystem.Auth.Services.Authentication;

public class CustomAuthenticationService : ICustomAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public CustomAuthenticationService(
        UserManager<User> userManager,
        ApplicationDbContext applicationDbContext,
        SignInManager<User> signInManager,
        ITokenService tokenService)
    {
        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<AuthStatusResponseDto?> GetAuthStatusFromToken(AuthStatusDto token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token.Token);

        var userIdClaim = jwtToken?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return new AuthStatusResponseDto { IsAuthenticated = false };
        }

        Guid? userId = null;
        if (Guid.TryParse(userIdClaim, out var parsedUserId))
        {
            userId = parsedUserId;
        }

        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return new AuthStatusResponseDto { IsAuthenticated = false };
        }

        var roles = await _userManager.GetRolesAsync(user);

        var claimsDict = jwtToken?.Claims?.ToDictionary(c => c.Type, c => c.Value) ?? new Dictionary<string, string>();

        return new AuthStatusResponseDto
        {
            Username = user.UserName,
            UserId = user.Id.ToString(),
            Email = user.Email,
            Roles = string.Join(", ", roles),
            AuthenticationType = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "auth_type")?.Value,
            IsEmailVerified = user.EmailConfirmed,
            ExpiresAt = jwtToken?.ValidTo > DateTime.MinValue ? jwtToken.ValidTo : DateTimeOffset.UtcNow.AddHours(1),
            AuthenticatedAt = jwtToken?.ValidFrom > DateTime.MinValue ? jwtToken.ValidFrom : DateTimeOffset.UtcNow,
            Claims = jwtToken?.Claims?.ToDictionary(
                c => c.Type,
                c => c.Type == "exp"
            ? DateTimeOffset.FromUnixTimeSeconds(long.Parse(c.Value)).ToString("yyyy-MM-dd HH:mm:ss 'UTC'")
            : c.Value
            ) ?? new Dictionary<string, string>(),
            IsAuthenticated = true
        };


    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto request)
    {
        // Fetch user with Role included
        var user = await _userManager.Users
            .Include(u => u.Role)  // Explicitly load Role
            .FirstOrDefaultAsync(u => u.UserName == request.Username);

        if (user == null || string.IsNullOrEmpty(user.UserName))
            return null;

        //Check if the email is confirmed
        if (!user.EmailConfirmed)
            return new LoginResponseDto 
            {
                IsEmailVerified = false
            };

        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);
        if (!result.Succeeded)
            return null;

        // Generate tokens
        var jwtToken = await _tokenService.GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Save refresh token
        await _tokenService.SaveRefreshTokenAsync(user, refreshToken);

        return new LoginResponseDto
        {
            JwtToken = jwtToken,
            RefreshToken = refreshToken,
            IsEmailVerified = true
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
