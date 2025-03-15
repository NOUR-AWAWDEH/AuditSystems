using AuditSystem.Auth.Dtos;

namespace AuditSystem.Auth.Services.Registration
{
    public interface IRegistrationService
    {
        Task RegisterAsync(RegisterDto request);
    }
}