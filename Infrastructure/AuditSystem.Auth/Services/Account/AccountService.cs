using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Account;

public class AccountService(UserManager<User> userManager) : IAccountService
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

    public Task<User?> FindByEmailAsync(string email)
    {
        return userManager.FindByEmailAsync(email);
    }
}
