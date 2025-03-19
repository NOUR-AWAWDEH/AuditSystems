using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Audit;

internal sealed class SpecialProjectValidator<T> : PropertyValidator<T, string?>, ISpecialProjectValidator
{
    public override string Name => "SpecialProjectValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Special project '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ISpecialProjectValidator : IPropertyValidator{ }