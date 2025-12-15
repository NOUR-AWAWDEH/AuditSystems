using System.ComponentModel.DataAnnotations;

namespace AuditSystem.Auth.Dtos.Password;

public class VerifyResetCodeRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Code { get; set; } = string.Empty;
}
