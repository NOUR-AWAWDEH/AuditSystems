using AuditSystem.Auth.Models;

namespace AuditSystem.Auth.Authentication
{
    public interface ITokenService
    {
        string GenerateAccessToken(ApplicationUser user);
        string GenerateJwtToken(ApplicationUser request);
        string GenerateRefreshToken();
        Task SaveRefreshTokenAsync(ApplicationUser user, string refreshToken);
    }
}
