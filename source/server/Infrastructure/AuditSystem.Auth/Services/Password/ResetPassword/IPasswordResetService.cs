using Microsoft.AspNetCore.Identity;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Auth.Services.Password.ResetPassword;

/// <summary>
/// Service for securely resetting user passwords using ASP.NET Identity tokens.
/// </summary>
public interface IPasswordResetService
{
    /// <summary>
    /// Resets a user's password using a secure token.
    /// The token must be generated via IPasswordTokenService and is validated by Identity.
    /// </summary>
    /// <param name="user">The user whose password is being reset.</param>
    /// <param name="token">The secure reset token sent to the user's email.</param>
    /// <param name="newPassword">The new password to set.</param>
    /// <returns>IdentityResult indicating success or failure with error details.</returns>
    Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
}