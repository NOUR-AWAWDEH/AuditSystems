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
}
