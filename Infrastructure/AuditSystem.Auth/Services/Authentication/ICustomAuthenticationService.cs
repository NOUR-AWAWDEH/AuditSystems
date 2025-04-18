using AuditSystem.Auth.Dtos.AuthStatus;
using AuditSystem.Auth.Dtos.Login;

namespace AuditSystem.Auth.Services.Authentication;

public interface ICustomAuthenticationService
{
    Task<AuthStatusResponseDto?> GetAuthStatusFromToken(AuthStatusRequestDto request);
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    Task<bool> RevokeAllRefreshTokensAsync(string userId);
    Task<bool> RevokeRefreshTokenAsync(string userId, string refreshToken);
    Task<bool> SignOutAsync();
}