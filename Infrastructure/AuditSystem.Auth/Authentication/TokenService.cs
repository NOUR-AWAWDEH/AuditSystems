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
    private readonly UserManager<User> _userManager;
    private readonly SigningCredentials _signingCredentials;
    private readonly int _expirationMinutes;

    public TokenService(IConfiguration config, UserManager<User> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

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

        _expirationMinutes = _jwtSettings.ExpirationMinutes;
    }

    public string GenerateJwtToken(User request)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, request.Id.ToString()),
            new Claim(ClaimTypes.Name, request.UserName ?? ""),
            new Claim(ClaimTypes.Email, request.Email ?? "")
        };

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
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
        if (user is User appUser)
        {
            appUser.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(appUser);
        }
    }
}
