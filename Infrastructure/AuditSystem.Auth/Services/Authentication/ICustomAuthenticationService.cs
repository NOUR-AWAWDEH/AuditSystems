using AuditSystem.Auth.Dtos;

namespace AuditSystem.Auth.Services.Authentication
{
    public interface ICustomAuthenticationService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto request);
        Task<bool> SignOutAsync();
    }
}