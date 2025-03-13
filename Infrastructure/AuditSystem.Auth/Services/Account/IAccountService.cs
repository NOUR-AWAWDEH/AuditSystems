using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Auth.Services.Account
{
    public interface IAccountService
    {
        Task<User?> FindByEmailAsync(string email);
    }
}