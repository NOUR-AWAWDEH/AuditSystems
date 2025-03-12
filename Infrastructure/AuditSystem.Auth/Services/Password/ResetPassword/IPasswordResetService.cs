using AuditSystem.Auth.Models;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Password.ResetPassword;

public interface IPasswordResetService
{
    Task<bool> VerifyResetCodeAsync(string email, string code);

    Task SaveResetCodeAsync(string email, string resetCode);

    Task ClearResetCodeAsync(ApplicationUser user);

    Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string newPassword);

    string GenerateResetCode();
}