using FluentValidation;

namespace AuditSystem.Application.Common.Validators;

public static class PropertyValidators
{
    public static IRuleBuilderOptions<T, string> ValidateRequiredString<T>(
        this IRuleBuilder<T, string> ruleBuilder, 
        int minLength, 
        int maxLength,
        string propertyName)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage($"{propertyName} is required.")
            .Length(minLength, maxLength)
            .WithMessage($"{propertyName} must be between {minLength} and {maxLength} characters");
    }

    public static IRuleBuilderOptions<T, Guid> ValidateRequiredGuid<T>(
        this IRuleBuilder<T, Guid> ruleBuilder,
        string propertyName)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage($"{propertyName} is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage($"{propertyName} cannot be an empty GUID.");
    }
}