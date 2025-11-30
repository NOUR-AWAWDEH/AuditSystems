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
        if (user == null) throw new ArgumentNullException(nameof(user));
        await RevokeAllRefreshTokensAsync(user.Id);
    }

    public async Task<bool> RevokeAllRefreshTokensAsync(Guid userId)
    {
        var tokens = await dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        if (!tokens.Any()) return false;

        tokens.ForEach(t => t.IsRevoked = true);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RevokeRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var token = await dbContext.RefreshTokens.FirstOrDefaultAsync(rt =>
            rt.UserId == userId && rt.Token == refreshToken && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow);

        if (token == null) return false;

        token.IsRevoked = true;
        await dbContext.SaveChangesAsync();
        return true;
    }
}
