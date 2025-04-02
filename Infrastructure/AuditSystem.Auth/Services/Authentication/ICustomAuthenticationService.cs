using AuditSystem.Auth.Dtos;

namespace AuditSystem.Auth.Services.Authentication;

public interface ICustomAuthenticationService
{
    Task<AuthStatusResponseDto?> GetAuthStatusFromToken(AuthStatusDto request);
    Task<LoginResponseDto?> LoginAsync(LoginDto request);
    Task<bool> SignOutAsync();
}