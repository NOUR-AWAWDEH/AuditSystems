using AuditSystem.Domain.Entities.Users;
namespace AuditSystem.Auth.Authentication;

public interface ITokenService
{
    Task <string>GenerateAccessTokenAsync(User user);
    Task <string>GenerateJwtTokenAsync(User request);
    string GenerateRefreshToken();
    Task<bool> SaveRefreshTokenAsync(User user, string refreshToken);
}
