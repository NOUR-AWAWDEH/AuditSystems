using AuditSystem.Contract.Dtos.Register;

namespace AuditSystem.Auth.Services.Registration
{
    public interface IRegistrationService
    {
        Task RegisterAsync(RegisterDto request, Guid adminUserId);
        Task VerificationEmailAsync(VerificationEmailDto request);
    }
}