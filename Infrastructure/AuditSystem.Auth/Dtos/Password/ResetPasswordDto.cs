using System.ComponentModel.DataAnnotations;

namespace AuditSystem.Auth.Dtos.Password;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Reset token is required.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm new password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Confirm new password must be between 6 and 100 characters.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
