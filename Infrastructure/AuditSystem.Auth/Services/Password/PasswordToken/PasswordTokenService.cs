using AuditSystem.Auth.Models;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Password.PasswordToken
{
    public class PasswordTokenService(UserManager<ApplicationUser> userManager) : IPasswordTokenService
    {
        public bool ValidateResetTokenAsync(ApplicationUser user, string token)
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

        public async Task<string> GenerateAndStorePasswordResetTokenAsync(ApplicationUser user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(1);  // Set expiration if needed

            // Save updated user data to database
            await userManager.UpdateAsync(user);

            return token;
        }

        public async Task<string?> GeneratePasswordResetTokenAsync(ApplicationUser request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return null;

            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null) return null;

            return await userManager.GeneratePasswordResetTokenAsync(user);
        }

    }
}
