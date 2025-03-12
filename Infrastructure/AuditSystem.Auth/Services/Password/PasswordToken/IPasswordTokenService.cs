using AuditSystem.Auth.Models;

namespace AuditSystem.Auth.Services.Password.PasswordToken;

public interface IPasswordTokenService
{
    bool ValidateResetTokenAsync(ApplicationUser user, string token);

    Task<string> GenerateAndStorePasswordResetTokenAsync(ApplicationUser user);

    Task<string?> GeneratePasswordResetTokenAsync(ApplicationUser user);

}