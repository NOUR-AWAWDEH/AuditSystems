using System.ComponentModel.DataAnnotations;

namespace AuditSystem.Auth.Dtos
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "Email cannot be that long.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public UserRole RoleName { get; set; } = UserRole.User;
    }
}