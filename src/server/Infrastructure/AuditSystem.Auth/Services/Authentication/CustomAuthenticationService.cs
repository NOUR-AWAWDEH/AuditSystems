using AuditSystem.Auth.Authentication;
using AuditSystem.Auth.Dtos.AuthStatus;
using AuditSystem.Auth.Dtos.Login;
using AuditSystem.Contract.Common;
using AuditSystem.DataAccess.Contexts;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuditSystem.Auth.Services.Authentication;

public class CustomAuthenticationService(
    UserManager<User> userManager,
    ApplicationDbContext applicationDbContext,
    SignInManager<User> signInManager,
    IHttpContextAccessor httpContextAccessor,
    ITokenService tokenService,
ILogger<CustomAuthenticationService> logger) : ICustomAuthenticationService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILogger<CustomAuthenticationService> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<AuthStatusResponseDto?> GetAuthStatusFromToken(AuthStatusRequestDto token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token.Token);

        var userIdClaim = jwtToken?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return new AuthStatusResponseDto { IsAuthenticated = false };
        }

        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return new AuthStatusResponseDto { IsAuthenticated = false };
        }

        var roles = jwtToken!.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);

        var ipAddress = _httpContextAccessor?.HttpContext?.Request?.Headers["X-Forwarded-For"].FirstOrDefault()
                       ?? _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        // Validate dates
        DateTimeOffset? expiresAtRaw = null;
        DateTimeOffset? authenticatedAtRaw = null;

        if (jwtToken.ValidTo != default && jwtToken.ValidTo.Year >= 1970 && jwtToken.ValidTo.Year <= 9999)
        {
            expiresAtRaw = jwtToken.ValidTo;
        }

        if (jwtToken.ValidFrom != default && jwtToken.ValidFrom.Year >= 1970 && jwtToken.ValidFrom.Year <= 9999)
        {
            authenticatedAtRaw = jwtToken.ValidFrom; // Uses nbf claim
        }
        else
        {
            // Fallback to iat claim
            var iatClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iat)?.Value;
            if (iatClaim != null && long.TryParse(iatClaim, out var iatUnixTime))
            {
                authenticatedAtRaw = DateTimeOffset.FromUnixTimeSeconds(iatUnixTime);
                if (authenticatedAtRaw?.Year is < 1970 or > 9999)
                {
                    authenticatedAtRaw = null;
                }
            }
        }

        return new AuthStatusResponseDto
        {
            Username = user.UserName,
            UserId = user.Id.ToString(),
            Email = user.Email,
            Roles = string.Join(", ", roles),
            AuthenticationType = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "auth_type")?.Value ?? "Bearer",
            IsEmailVerified = user.EmailConfirmed,
            ExpiresAtRaw = expiresAtRaw,
            AuthenticatedAtRaw = authenticatedAtRaw,
            IpAddress = ipAddress,
            IsAuthenticated = true,
            ExpiresAt = expiresAtRaw?.ToString("MMM dd, yyyy hh:mm:ss tt 'UTC'"),
            AuthenticatedAt = authenticatedAtRaw?.ToString("MMM dd, yyyy hh:mm:ss tt 'UTC'")
        };
    }


    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        // 1. Validate input
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            _logger.LogWarning("Login attempt with missing username or password.");
            return ApiResponse<LoginResponseDto>.BadRequest("Username and password are required.");
        }

        // 2. Fetch user with custom Role navigation property
        var user = await _userManager.Users
            .Include(u => u.Role) // Keep this since you're using a custom Role relationship
            .FirstOrDefaultAsync(u => u.UserName == request.Username);

        if (user == null)
        {
            _logger.LogWarning("User not found with username: {Username}", request.Username);
            return ApiResponse<LoginResponseDto>.BadRequest("Invalid username or password.");
        }

        // 3. Check if the email is confirmed
        if (!user.EmailConfirmed)
        {
            _logger.LogWarning("Unverified email login attempt for user: {Username}", request.Username);
            return ApiResponse<LoginResponseDto>.BadRequest("Email not verified. Please verify your email before logging in.");
        }

        // 4. Check password using SignInManager
        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);
        if (!result.Succeeded)
        {
            string errorMessage = "Invalid username or password.";

            if (result.IsLockedOut)
            {
                errorMessage = "Account is locked. Please try again later or contact support.";
                _logger.LogWarning("Account locked for user: {Username}", request.Username);
            }
            else if (result.IsNotAllowed)
            {
                errorMessage = "Login not allowed. Please contact support.";
                _logger.LogWarning("Login not allowed for user: {Username}", request.Username);
            }
            else if (result.RequiresTwoFactor)
            {
                errorMessage = "Two-factor authentication required.";
                _logger.LogWarning("Two-factor authentication required for user: {Username}", request.Username);
            }
            else
            {
                _logger.LogWarning("Invalid password for user: {Username}", request.Username);
            }

            return ApiResponse<LoginResponseDto>.BadRequest(errorMessage);
        }

        // 5. Generate JWT and Refresh Token
        var jwtToken = await _tokenService.GenerateJwtTokenAsync(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // 6. Save the refresh token securely
        var tokenSaveResult = await _tokenService.SaveRefreshTokenAsync(user, refreshToken);
        if (!tokenSaveResult)
        {
            _logger.LogError("Failed to save refresh token for user: {Username}", request.Username);
            return ApiResponse<LoginResponseDto>.InternalServerError("Failed to generate tokens. Please try again.");
        }

        // 7. Construct the response DTO
        var loginResponse = new LoginResponseDto
        {
            JwtToken = jwtToken,
            RefreshToken = refreshToken,
            IsEmailVerified = true
        };

        _logger.LogInformation("User {Username} logged in successfully.", request.Username);
        return ApiResponse<LoginResponseDto>.Ok(loginResponse, "Login successful.");
    }

    public async Task<bool> RevokeAllRefreshTokensAsync(string userId)
    {
        _logger.LogInformation("RevokeAllRefreshTokensAsync called with userId: {userId}", userId);
        if (!Guid.TryParse(userId, out var userIdGuid))
        {
            _logger.LogWarning("Invalid userId format: {userId}", userId);
            return false;
        }

        var tokens = await _applicationDbContext.RefreshTokens
            .Where(rt => rt.UserId == userIdGuid && !rt.IsRevoked)
            .ToListAsync();

        if (tokens.Count == 0)
        {
            _logger.LogInformation("No active refresh tokens found for userId: {userId}", userId);
            return false;
        }

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
        }

        await _applicationDbContext.SaveChangesAsync();
        _logger.LogInformation("All refresh tokens revoked successfully for userId: {userId}", userId);
        return true;
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

    public async Task<ApiResponse<AuthResultDto>> RefreshTokenAsync(string refreshToken)
    {
        var token = await _applicationDbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt =>
                rt.Token == refreshToken &&
                !rt.IsRevoked &&
                rt.ExpiresAt > DateTime.UtcNow);

        if (token == null || token.User == null)
            return ApiResponse<AuthResultDto>.BadRequest("Invalid or expired refresh token");

        var user = await _userManager.FindByIdAsync(token.UserId.ToString());
        if (user == null)
            return ApiResponse<AuthResultDto>.BadRequest("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        if (roles == null || roles.Count == 0)
            return ApiResponse<AuthResultDto>.BadRequest("User must have an assigned role");


        // Revoke old token
        token.IsRevoked = true;

        // Generate new tokens
        var newJwt = await _tokenService.GenerateJwtTokenAsync(user);
        var newRefresh = _tokenService.GenerateRefreshToken();

        var saveResult = await _tokenService.SaveRefreshTokenAsync(user, newRefresh);
        if (!saveResult)
            return ApiResponse<AuthResultDto>.InternalServerError("Failed to persist refresh token");

        await _applicationDbContext.SaveChangesAsync();

        var result = new AuthResultDto
        {
            JwtToken = newJwt,
            RefreshToken = newRefresh
        };

        return ApiResponse<AuthResultDto>.Ok(result, "Token refreshed successfully");
    }

}
