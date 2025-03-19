using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Common;

internal sealed class RatingValidator<T> : PropertyValidator<T, string?>, IRatingValidator
{
    public override string Name => "RatingValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Rating '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IRatingValidator : IPropertyValidator{}
