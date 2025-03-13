using AuditSystem.Domain.Entities.Users;
using AuditSystem.Auth.Services.Account;
using Microsoft.AspNetCore.Identity;

public class AccountService(UserManager<User> userManager) : IAccountService
{
    public Task<User?> FindByEmailAsync(string email)
    {
        return userManager.FindByEmailAsync(email);
    }
}