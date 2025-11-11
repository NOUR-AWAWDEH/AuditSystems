using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AuditSystem.Auth.Dtos.Password;

public class PasswordComplexityAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return ValidationResult.Success; // Let [Required] handle this if needed
            }

            // Consider making these requirements configurable (e.g., via constructor parameters)
            var hasUpperCase = new Regex(@"[A-Z]+").IsMatch(password);
            var hasLowerCase = new Regex(@"[a-z]+").IsMatch(password);
            var hasNumber = new Regex(@"[0-9]+").IsMatch(password);
            var hasSpecialChar = new Regex(@"[!@#$%^&*()_+=\[\]{};':""\\|,.<>\/?]+").IsMatch(password);

            if (!hasUpperCase || !hasLowerCase || !hasNumber || !hasSpecialChar)
            {
                return new ValidationResult(ErrorMessage ?? "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
            }
        }

        return ValidationResult.Success;
    }
}