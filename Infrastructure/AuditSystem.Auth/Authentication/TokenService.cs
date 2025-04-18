using AuditSystem.DataAccess.Contexts;
using AuditSystem.Domain.Entities.Auth;
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

    public TokenService(IConfiguration config, UserManager<User> userManager, ApplicationDbContext applicationDbContext)
    {
        _jwtSettings = config.GetSection("JwtSettings").Get<JwtSettings>()
                       ?? throw new InvalidOperationException("JwtSettings section is missing or invalid in configuration.");

        var missingFields = new List<string>();
        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            missingFields.Add("JwtSettings:Key");
        }
        if (string.IsNullOrEmpty(_jwtSettings.Issuer))
        {
            missingFields.Add("JwtSettings:Issuer");
        }
        if (string.IsNullOrEmpty(_jwtSettings.Audience))
        {
            missingFields.Add("JwtSettings:Audience");
        }
        if (_jwtSettings.ExpirationMinutes <= 0)
        {
            missingFields.Add("JwtSettings:ExpirationMinutes");
        }

        if (missingFields.Any())
        {
            throw new InvalidOperationException($"Missing JwtSettings fields: {string.Join(", ", missingFields)}");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        _applicationDbContext = applicationDbContext;
    }


    public string GenerateJwtToken(User request)
    {
        if (request.Role == null)
        {
            throw new InvalidOperationException("User must have an assigned role.");
        }

        var claims = new List<Claim>
{
            new Claim(ClaimTypes.NameIdentifier, request.Id.ToString()),
            new Claim(ClaimTypes.Name, request.UserName ?? ""),
            new Claim(ClaimTypes.Email, request.Email ?? ""),
            new Claim(ClaimTypes.Role, request.Role.Name),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Issued At (login time)
            new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Not Before
            new Claim("login_time", DateTimeOffset.UtcNow.ToString("o")), // Custom login time claim (ISO 8601 format)
            new Claim("auth_type", "Bearer") // To fix authenticationType: null
        };

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: _signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateAccessToken(User user)
    {
        return GenerateJwtToken(user);
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

    public async Task SaveRefreshTokenAsync(User user, string refreshToken)
    {
        // Optional: Validate inputs
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7), // Adjust expiration as needed
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        _applicationDbContext.RefreshTokens.Add(refreshTokenEntity);
        await _applicationDbContext.SaveChangesAsync();
    }
}
