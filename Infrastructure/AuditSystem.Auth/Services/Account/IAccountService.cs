using AuditSystem.Auth.Models;

namespace AuditSystem.Auth.Services.Account
{
    public interface IAccountService
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
    }
}