using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Account;

public interface IAccountService
{
    Task<User?> FindByEmailAsync(string email);
    Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
}