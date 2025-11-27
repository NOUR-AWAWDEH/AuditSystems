using Microsoft.AspNetCore.Identity;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Auth.Services.Account;

public class AccountService(UserManager<ApplicationUser> userManager) : IAccountService
{
    public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
    {
        var user = await userManager.FindByIdAsync(uid);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User not found");
        }
        return await userManager.ConfirmEmailAsync(user, token);
    }

    public Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        return userManager.FindByEmailAsync(email);
    }
}
