using AuditSystem.DataAccess.Contexts;
using AuditSystem.Domain.Entities.Auth;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuditSystem.Auth.Authentication;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly SigningCredentials _signingCredentials;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(
        IConfiguration config,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext applicationDbContext)
    {
        _jwtSettings = new JwtSettings
        {
            Key = config["JWT_KEY"] ?? "",
            Issuer = config["JWT_ISSUER"] ?? "",
            Audience = config["JWT_AUDIENCE"] ?? "",
            ExpirationMinutes = int.TryParse(config["JWT_EXPIRATION_MINUTES"], out var minutes) ? minutes : 0
        };

        var missingFields = new List<string>();
        if (string.IsNullOrWhiteSpace(_jwtSettings.Key)) missingFields.Add("JWT_KEY");
        if (string.IsNullOrWhiteSpace(_jwtSettings.Issuer)) missingFields.Add("JWT_ISSUER");
        if (string.IsNullOrWhiteSpace(_jwtSettings.Audience)) missingFields.Add("JWT_AUDIENCE");
        if (_jwtSettings.ExpirationMinutes <= 0) missingFields.Add("JWT_EXPIRATION_MINUTES");

        if (missingFields.Any())
            throw new InvalidOperationException($"Missing JWT config values: {string.Join(", ", missingFields)}");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
    }

    public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "User cannot be null.");

        var roles = await _userManager.GetRolesAsync(user);

        if (roles == null || !roles.Any())
            throw new InvalidOperationException("User must have at least one assigned role.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? ""),
            new(ClaimTypes.Email, user.Email ?? ""),
            new(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Nbf,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new("login_time", DateTimeOffset.UtcNow.ToString("o")),
            new("auth_type", "Bearer"),
            new("AspNet.Identity.SecurityStamp", user.SecurityStamp ?? "")
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: _signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
    {
        return await GenerateJwtTokenAsync(user);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<bool> SaveRefreshTokenAsync(ApplicationUser user, string refreshToken)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrEmpty(refreshToken))
            throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        _applicationDbContext.RefreshTokens.Add(refreshTokenEntity);
        await _applicationDbContext.SaveChangesAsync();

        return true;
    }
}