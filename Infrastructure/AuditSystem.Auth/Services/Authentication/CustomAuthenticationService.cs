using AuditSystem.Auth.Authentication;
using AuditSystem.Auth.Dtos.AuthStatus;
using AuditSystem.Auth.Dtos.Login;
using AuditSystem.DataAccess.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuditSystem.Auth.Services.Authentication;

public class CustomAuthenticationService : ICustomAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<CustomAuthenticationService> _logger;

    public CustomAuthenticationService(
        UserManager<User> userManager,
        ApplicationDbContext applicationDbContext,
        SignInManager<User> signInManager,
        ITokenService tokenService,
        ILogger<CustomAuthenticationService> logger)
    {
        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<AuthStatusResponseDto?> GetAuthStatusFromToken(AuthStatusRequestDto token)
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

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        // Fetch user with Role included
        var user = await _userManager.Users
            .Include(u => u.Role)  // Explicitly load Role
            .FirstOrDefaultAsync(u => u.UserName == request.Username);

        if (user == null || string.IsNullOrEmpty(user.UserName))
        {
            _logger.LogError("User not found with username: {Username}", request.Username);
            return null;
        }

        // Check if the email is confirmed
        if (!user.EmailConfirmed)
        {
            return new LoginResponseDto
            {
                IsEmailVerified = false
            };
        }

        // Check password
        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);
        if (!result.Succeeded)
        {
            _logger.LogError("Sign-in failed for user: {Username}", request.Username);
            return null;
        }

        // Generate tokens
        var jwtToken = _tokenService.GenerateJwtToken(user);
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


    public async Task<bool> RevokeRefreshTokenAsync(string userId, string refreshToken)
    {
        _logger.LogInformation("RevokeRefreshTokenAsync called with userId: {userId} and refreshToken: {refreshToken}", userId, refreshToken);
        if (!Guid.TryParse(userId, out var userIdGuid))
        {
            _logger.LogWarning("Invalid userId format: {userId}", userId);
            return false;
        }

        var token = await _applicationDbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userIdGuid
                                    && rt.Token == refreshToken
                                    && !rt.IsRevoked
                                    && rt.ExpiresAt > DateTime.UtcNow);

        if (token == null)
        {
            _logger.BeginScope("Token not found or already revoked for userId: {userId}", userId);
            return false;
        }

        token.IsRevoked = true;
        await _applicationDbContext.SaveChangesAsync();
        _logger.LogInformation("Refresh token revoked successfully for userId: {userId}", userId);
        return true;
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
