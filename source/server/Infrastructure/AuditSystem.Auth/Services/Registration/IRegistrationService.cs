using AuditSystem.Auth.Dtos.Register;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Auth.Services.Registration
{
    public interface IRegistrationService
    {
        Task RegisterAsync(RegisterDto request, string scheme);
        Task VerificationEmailAsync(VerificationEmailDto request, string scheme);
    }
}