using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Password.ResetPassword;

public interface IPasswordResetService
{
    Task<bool> VerifyResetCodeAsync(string email, string code);
    Task SaveResetCodeAsync(string email, string resetCode);
    Task ClearResetCodeAsync(User user);
    Task<IdentityResult> ResetPasswordAsync(User user, string newPassword);
    string GenerateResetCode();
}