using Microsoft.AspNetCore.Identity;

namespace AuditSystem.Auth.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }

        public string? PasswordResetCode { get; set; }

        public DateTime? PasswordResetCodeExpiration { get; set; }

        public string PasswordResetToken { get; internal set; } = string.Empty;

        public DateTime PasswordResetTokenExpiration { get; internal set; }

        public DateTime? LastLogoutTime { get; set; }
    }
}