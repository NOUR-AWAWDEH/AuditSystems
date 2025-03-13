using System.ComponentModel.DataAnnotations;
namespace AuditSystem.Auth.Dtos
{
    public class VerifyResetCodeDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public required string Email { get; set; } = string.Empty;
        
        public required string Code { get; set; } = string.Empty;
    }
}
