using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Account;

public class AccountService(UserManager<User> userManager) : IAccountService
{
    public Task<User?> FindByEmailAsync(string email)
    {
        return userManager.FindByEmailAsync(email);
    }
}