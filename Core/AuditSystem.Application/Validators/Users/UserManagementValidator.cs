using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Users;

internal sealed class UserManagementValidator<T> : PropertyValidator<T, string?>, IUserManagementValidator
{
    public override string Name => "UserManagementValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "User '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IUserManagementValidator : IPropertyValidator {}
