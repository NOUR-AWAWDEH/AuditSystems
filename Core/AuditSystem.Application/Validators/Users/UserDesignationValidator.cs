using FluentValidation.Validators;
using FluentValidation;

namespace AuditSystem.Application.Validators.Users;

internal sealed class UserDesignationValidator<T> : PropertyValidator<T, string?>, IUserDesignationValidator
{
    public override string Name => "UserDesignationValidator";
    protected override string GetDefaultMessageTemplate(string errorCode)
         => "UserDesignation '{PropertyValue}' is not valid.";
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }
        return Guid.TryParse(value, out _);
    }
}
public interface IUserDesignationValidator : IPropertyValidator
{
}