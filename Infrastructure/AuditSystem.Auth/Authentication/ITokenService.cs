namespace AuditSystem.Auth.Authentication;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateJwtToken(User request);
    string GenerateRefreshToken();
    Task SaveRefreshTokenAsync(User user, string refreshToken);
}
