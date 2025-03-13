using AuditSystem.Auth.Dtos;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Registration
{
    public interface IRegistrationService
    {
        Task RegisterAsync(RegisterDto request);
    }
}