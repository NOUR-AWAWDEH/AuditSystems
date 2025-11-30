using Microsoft.AspNetCore.Identity;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Auth.Services.Account;

public interface IAccountService
{
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
    Task RevokeAllRefreshTokensAsync(ApplicationUser user);
    Task<bool> RevokeAllRefreshTokensAsync(Guid userId);
    Task<bool> RevokeRefreshTokenAsync(Guid userId, string refreshToken);
}