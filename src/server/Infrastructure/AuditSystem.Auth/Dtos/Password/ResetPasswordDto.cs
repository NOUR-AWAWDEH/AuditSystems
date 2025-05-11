using System.ComponentModel.DataAnnotations;

namespace AuditSystem.Auth.Dtos.Password;

public class ResetPasswordDto : IValidatableObject
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Reset token is required.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    [PasswordComplexity(ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm new password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Confirm new password must be between 6 and 100 characters.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (NewPassword != ConfirmNewPassword)
        {
            yield return new ValidationResult("New password and confirm new password do not match.", new[] { nameof(NewPassword), nameof(ConfirmNewPassword) });
        }
    }
}

