using AuditSystem.Auth.Dtos.AuthStatus;
using AuditSystem.Auth.Dtos.Login;
using AuditSystem.Contract.Common;
namespace AuditSystem.Auth.Services.Authentication;

public interface ICustomAuthenticationService
{
    Task<AuthStatusResponseDto?> GetAuthStatusFromToken(AuthStatusRequestDto request);
    Task<ApiResponse<AuthResultDto>> RefreshTokenAsync(string refreshToken);
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    Task<bool> RevokeAllRefreshTokensAsync(string userId);
    Task<bool> RevokeRefreshTokenAsync(string userId, string refreshToken);
    Task<bool> SignOutAsync();
}