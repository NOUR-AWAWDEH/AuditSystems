using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Services.Password.PasswordToken;

/// <summary>
/// Service for generating secure password reset tokens using ASP.NET Identity.
/// Tokens are cryptographically secure, time-limited, and single-use.
/// </summary>
public class PasswordTokenService : IPasswordTokenService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public PasswordTokenService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Generates a cryptographically secure password reset token for the specified user.
    /// The token is URL-safe and should be sent to the user via email.
    /// Token expiration is configured in Identity options (default: 1 day).
    /// </summary>
    /// <param name="user">The user requesting a password reset.</param>
    /// <returns>A secure token string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when user is null.</exception>
    public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    /// <summary>
    /// Verifies if a password reset token is valid for the specified user.
    /// </summary>
    /// <param name="user">The user to verify the token for.</param>
    /// <param name="token">The token to verify.</param>
    /// <returns>True if the token is valid; otherwise, false.</returns>
    public async Task<bool> VerifyPasswordResetTokenAsync(ApplicationUser user, string token)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrWhiteSpace(token))
            return false;

        return await _userManager.VerifyUserTokenAsync(
            user,
            _userManager.Options.Tokens.PasswordResetTokenProvider,
            UserManager<ApplicationUser>.ResetPasswordTokenPurpose,
            token);
    }
}
