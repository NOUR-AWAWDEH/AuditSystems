using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Password.PasswordToken;

public class PasswordTokenService(UserManager<User> userManager) : IPasswordTokenService
{
    public  bool ValidateResetTokenAsync(User user, string token)
    {
        if (user == null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        if (user.PasswordResetToken == null)
        {
            return false;
        }

        if (user.PasswordResetToken != token || user.PasswordResetTokenExpiration < DateTime.UtcNow)
        {
            return false;
        }

        return true;
    }

    public async Task<string> GenerateAndStorePasswordResetTokenAsync(User user)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        user.PasswordResetToken = token;
        user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(1);  // Set expiration if needed

        // Save updated user data to database
        await userManager.UpdateAsync(user);

        return token;
    }

    public async Task<string?> GeneratePasswordResetTokenAsync(User request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return null;

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null) return null;

        return await userManager.GeneratePasswordResetTokenAsync(user);
    }
}
