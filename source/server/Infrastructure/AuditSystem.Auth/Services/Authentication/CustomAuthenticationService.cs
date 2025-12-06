using AuditSystem.Auth.Authentication;
using AuditSystem.Auth.Dtos.AuthStatus;
using AuditSystem.Auth.Dtos.Login;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Contract.Common;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices;
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
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ApplicationDbContext db,
    ITokenService tokenService,
    IUserSessionManager userSessionManager,
    IAccountService accountService,
    IHttpContextAccessor httpContextAccessor,
    ILogger<CustomAuthenticationService> logger) : ICustomAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ApplicationDbContext _db = db;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUserSessionManager _userSessionManager = userSessionManager;
    private readonly IAccountService _accountService = accountService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ILogger<CustomAuthenticationService> _logger = logger;

    #region Login

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            _logger.LogWarning("Login attempt with missing username or password.");
            return ApiResponse<LoginResponseDto>.BadRequest("Username and password are required.");
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
        if (user == null)
        {
            _logger.LogWarning("User not found: {Username}", request.Username);
            return ApiResponse<LoginResponseDto>.BadRequest("Invalid username or password.");
        }

        if (!user.EmailConfirmed)
            return ApiResponse<LoginResponseDto>.BadRequest("Email not verified.");

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, false);
        if (!result.Succeeded)
            return ApiResponse<LoginResponseDto>.BadRequest("Invalid username or password.");

        // Generate JWT & refresh token
        var jwtToken = await _tokenService.GenerateJwtTokenAsync(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        if (!await _tokenService.SaveRefreshTokenAsync(user, refreshToken))
        {
            _logger.LogError("Failed to save refresh token for {Username}", request.Username);
            return ApiResponse<LoginResponseDto>.InternalServerError("Failed to generate tokens.");
        }

        // --- Start user session via UserSessionManager ---
        var appUser = await _db.AppUsers.FirstOrDefaultAsync(au => au.IdentityUserId == user.Id);
        if (appUser != null)
        {
            await _userSessionManager.StartSessionAsync(appUser);
        }
        else
        {
            _logger.LogWarning("AppUser not found for IdentityUser {IdentityUserId}", user.Id);
            return ApiResponse<LoginResponseDto>.BadRequest("Invalid username, password, or company.");
        }

        return ApiResponse<LoginResponseDto>.Ok(new LoginResponseDto
        {
            JwtToken = jwtToken,
            RefreshToken = refreshToken,
            IsEmailVerified = appUser.IsActive,
            CompanyId = appUser.CompanyId,
        }, "Login successful.");
    }

    #endregion

    #region SignOut

    public async Task<bool> SignOutAsync()
    {
        try
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdStr, out var identityUserId))
                return false;


            // 1. Update security stamp to invalidate all JWTs
            var user = await _userManager.FindByIdAsync(userIdStr);
            if (user != null)
            {
                await _userManager.UpdateSecurityStampAsync(user); // this invalidates all JWTs
            }

            // 2. Revoke ALL refresh tokens
            await _accountService.RevokeAllRefreshTokensAsync(identityUserId);

            // 3. End business session
            var appUser = await _db.AppUsers
                .FirstOrDefaultAsync(au => au.IdentityUserId == identityUserId);

            if (appUser != null)
            {
                await _userSessionManager.EndSessionAsync(appUser);
            }

            // 4. Sign out Identity
            await _signInManager.SignOutAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error signing out.");
            return false;
        }
    }


    #endregion


    #region Auth Status

    public async Task<AuthStatusResponseDto?> GetAuthStatusFromToken(AuthStatusRequestDto token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token.Token);

        var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return new AuthStatusResponseDto { IsAuthenticated = false };

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return new AuthStatusResponseDto { IsAuthenticated = false };

        var roles = jwtToken?.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);

        var ipAddress = _httpContextAccessor.HttpContext?.Request?.Headers["X-Forwarded-For"].FirstOrDefault()
                        ?? _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        var expiresAt = jwtToken?.ValidTo > DateTime.MinValue ? jwtToken.ValidTo : (DateTime?)null;
        var authenticatedAt = jwtToken?.ValidFrom > DateTime.MinValue ? jwtToken.ValidFrom : (DateTime?)null;

        var appUser = await _db.AppUsers.FirstOrDefaultAsync(au => au.IdentityUserId == user.Id);

        return new AuthStatusResponseDto
        {
            Username = user.UserName,
            UserId = user.Id.ToString(),
            Email = user.Email,
            CompanyId = appUser?.CompanyId.ToString(),
            Roles = string.Join(", ", roles!),
            AuthenticationType = jwtToken?.Claims.FirstOrDefault(c => c.Type == "auth_type")?.Value ?? "Bearer",
            IsEmailVerified = user.EmailConfirmed,
            ExpiresAtRaw = expiresAt,
            AuthenticatedAtRaw = authenticatedAt,
            IpAddress = ipAddress,
            IsAuthenticated = true,
            ExpiresAt = expiresAt?.ToString("MMM dd, yyyy hh:mm:ss tt 'UTC'"),
            AuthenticatedAt = authenticatedAt?.ToString("MMM dd, yyyy hh:mm:ss tt 'UTC'")
        };
    }

    #endregion

    #region Refresh Token

    public async Task<ApiResponse<AuthResultDto>> RefreshTokenAsync(string refreshToken)
    {
        var token = await _db.RefreshTokens.Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow);

        if (token == null || token.User == null)
            return ApiResponse<AuthResultDto>.BadRequest("Invalid or expired refresh token.");

        var user = await _userManager.FindByIdAsync(token.UserId.ToString());
        if (user == null)
            return ApiResponse<AuthResultDto>.BadRequest("User not found.");

        // Remove old token
        _db.RefreshTokens.Remove(token);

        var newJwt = await _tokenService.GenerateJwtTokenAsync(user);
        var newRefresh = _tokenService.GenerateRefreshToken();
        if (!await _tokenService.SaveRefreshTokenAsync(user, newRefresh))
            return ApiResponse<AuthResultDto>.InternalServerError("Failed to save refresh token.");

        await _db.SaveChangesAsync();

        return ApiResponse<AuthResultDto>.Ok(new AuthResultDto
        {
            JwtToken = newJwt,
            RefreshToken = newRefresh
        }, "Token refreshed successfully.");
    }

    #endregion

    #region Revoke Tokens

    public async Task<bool> RevokeAllRefreshTokensAsync(string userId)
    {
        if (!Guid.TryParse(userId, out var uid)) return false;
        return await _accountService.RevokeAllRefreshTokensAsync(uid);
    }

    public async Task<bool> RevokeRefreshTokenAsync(string userId, string refreshToken)
    {
        if (!Guid.TryParse(userId, out var uid)) return false;
        return await _accountService.RevokeRefreshTokenAsync(uid, refreshToken);
    }

    #endregion
}
