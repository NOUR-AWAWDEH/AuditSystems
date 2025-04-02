using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Risks;

internal sealed class RiskFactorValidator<T> : PropertyValidator<T, string?>, IRiskFactorValidator
{
    public override string Name => "RiskFactorValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "RiskFactor '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IRiskFactorValidator : IPropertyValidator { }
