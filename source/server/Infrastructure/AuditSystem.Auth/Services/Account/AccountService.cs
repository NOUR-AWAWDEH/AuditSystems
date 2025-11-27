using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuditSystem.Domain.Entities.Users;
using AuditSystem.DataAccess.Contexts;

namespace AuditSystem.Auth.Services.Account;

public class AccountService(
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext dbContext) : IAccountService
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

    /// <summary>
    /// Revokes all refresh tokens for a user, effectively logging them out of all sessions.
    /// This should be called after password reset for security.
    /// </summary>
    public async Task RevokeAllRefreshTokensAsync(ApplicationUser user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var refreshTokens = await dbContext.RefreshTokens
            .Where(rt => rt.UserId == user.Id && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in refreshTokens)
        {
            token.IsRevoked = true;
        }

        await dbContext.SaveChangesAsync();
    }
}
