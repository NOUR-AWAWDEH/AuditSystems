using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Auth.Authentication;

public interface ITokenService
{
    Task<string> GenerateAccessToken(User user);
    Task<string> GenerateJwtToken(User request);
    string GenerateRefreshToken();
    Task SaveRefreshTokenAsync(User user, string refreshToken);
}
