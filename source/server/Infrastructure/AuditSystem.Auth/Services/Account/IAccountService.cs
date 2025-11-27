using Microsoft.AspNetCore.Identity;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Auth.Services.Account;

public interface IAccountService
{
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
}