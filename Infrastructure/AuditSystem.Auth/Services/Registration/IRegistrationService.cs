using AuditSystem.Auth.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Auth.Services.Registration
{
    public interface IRegistrationService
    {
        Task RegisterAsync(RegisterDto request, string scheme, IUrlHelper urlHelper);
    }
}