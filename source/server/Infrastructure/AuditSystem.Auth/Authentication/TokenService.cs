using AuditSystem.DataAccess.Contexts;
using AuditSystem.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Auth.Authentication;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly SigningCredentials _signingCredentials;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly UserManager<User> _userManager;


    public TokenService(IConfiguration config, UserManager<User> userManager, ApplicationDbContext applicationDbContext)
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
        {
            throw new InvalidOperationException($"Missing JWT config values: {string.Join(", ", missingFields)}");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
    }



    public async Task<string> GenerateJwtTokenAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "User cannot be null.");

        var roles = await _userManager.GetRolesAsync(user);

        if (roles == null || !roles.Any())
            throw new InvalidOperationException("User must have at least one assigned role.");

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName ?? ""),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        new Claim("login_time", DateTimeOffset.UtcNow.ToString("o")),
        new Claim("auth_type", "Bearer")
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


    public async Task<string> GenerateAccessTokenAsync(User user)
    {
        return await GenerateJwtTokenAsync(user);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<bool> SaveRefreshTokenAsync(User user, string refreshToken)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        _applicationDbContext.RefreshTokens.Add(refreshTokenEntity);
        await _applicationDbContext.SaveChangesAsync();

        return true; 
    }

}