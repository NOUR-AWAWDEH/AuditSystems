using System.ComponentModel.DataAnnotations;

namespace AuditSystem.Auth.Dtos.Register;

public class RegisterDto
{
    // ========================= AUTH =========================

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Username contains invalid characters.")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // ========================= PROFILE =========================

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;

    // ========================= SYSTEM =========================

    [Required]
    [StringLength(50)]
    public string RoleName { get; set; } = string.Empty;

    [Required]
    public Guid CompanyId { get; set; }

    [Required] // ✅ REQUIRED BY AppUser
    public Guid JobTitleId { get; set; }
}
