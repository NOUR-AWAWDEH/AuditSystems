using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Auth.Services.Password.PasswordToken;

/// <summary>
/// Service for generating secure password reset tokens using ASP.NET Identity.
/// </summary>
public interface IPasswordTokenService
{
    /// <summary>
    /// Generates a cryptographically secure password reset token for the specified user.
    /// This token is time-limited and can only be used once.
    /// </summary>
    /// <param name="user">The user requesting a password reset.</param>
    /// <returns>A URL-safe token string to be sent via email.</returns>
    Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

    /// <summary>
    /// Verifies if a password reset token is valid for the specified user.
    /// This method checks the token without consuming it (unless the token type implies consumption, but usually verify is safe).
    /// </summary>
    /// <param name="user">The user to verify the token for.</param>
    /// <param name="token">The token to verify.</param>
    /// <returns>True if the token is valid; otherwise, false.</returns>
    Task<bool> VerifyPasswordResetTokenAsync(ApplicationUser user, string token);
}