using AuditSystem.Auth.Models;
using AuditSystem.Auth.Services.Account;
using Microsoft.AspNetCore.Identity;

public class AccountService(UserManager<ApplicationUser> userManager) : IAccountService
{
    public Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        return userManager.FindByEmailAsync(email);
    }
}