namespace AuditSystem.Auth.Services.Password.PasswordToken;

public interface IPasswordTokenService
{
    bool ValidateResetTokenAsync(User user, string token);
    Task<string> GenerateAndStorePasswordResetTokenAsync(User user);
    Task<string?> GeneratePasswordResetTokenAsync(User user);
}