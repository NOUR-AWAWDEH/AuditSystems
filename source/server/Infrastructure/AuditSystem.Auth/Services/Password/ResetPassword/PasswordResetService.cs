using AuditSystem.Auth.Services.Password.PasswordToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.Auth.Services.Password.ResetPassword;

public class PasswordResetService(UserManager<User> userManager, IPasswordTokenService passwordTokenService) : IPasswordResetService
{

    public async Task<IdentityResult> ResetPasswordAsync(User user, string newPassword)
    {
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        var token = await passwordTokenService.GeneratePasswordResetTokenAsync(user);
        if (string.IsNullOrEmpty(token))
            return IdentityResult.Failed(new IdentityError { Description = "Failed to generate reset token." });

        return await userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task SaveResetCodeAsync(string email, string resetCode)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.Email == email);

        if (user == null)
            throw new InvalidOperationException("User not found.");

        user.PasswordResetCode = resetCode;
        user.PasswordResetCodeExpiration = DateTime.UtcNow.AddMinutes(10);

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException("Failed to update reset code.");
    }

    public async Task<bool> VerifyResetCodeAsync(string email, string code)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null || string.IsNullOrEmpty(user.PasswordResetCode))
            return false;

        return user.PasswordResetCode == code && user.PasswordResetCodeExpiration >= DateTime.UtcNow;
    }

    public string GenerateResetCode()
    {
        int length = 5;
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var code = new string(Enumerable.Range(0, length)
                                        .Select(_ => validChars[random.Next(validChars.Length)])
                                        .ToArray());
        return code;
    }

    public async Task ClearResetCodeAsync(User user)
    {
        user.PasswordResetCode = null;
        user.PasswordResetCodeExpiration = null;
        await userManager.UpdateAsync(user);
    }
}
