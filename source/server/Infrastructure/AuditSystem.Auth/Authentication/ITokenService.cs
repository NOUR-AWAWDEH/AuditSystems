using AuditSystem.Domain.Entities.Users;
namespace AuditSystem.Auth.Authentication;

public interface ITokenService
{
    Task <string>GenerateAccessTokenAsync(ApplicationUser user);
    Task <string>GenerateJwtTokenAsync(ApplicationUser request);
    string GenerateRefreshToken();
    Task<bool> SaveRefreshTokenAsync(ApplicationUser user, string refreshToken);
}
