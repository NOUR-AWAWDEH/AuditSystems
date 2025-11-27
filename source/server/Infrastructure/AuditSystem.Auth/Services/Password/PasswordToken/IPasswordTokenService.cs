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
}