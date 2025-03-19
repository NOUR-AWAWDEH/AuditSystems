using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.RiskControls;

internal sealed class RiskControlsValidator<T> : PropertyValidator<T, string?>, IRiskControlsValidator
{
    public override string Name => "RiskControlsValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "RiskControls '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IRiskControlsValidator : IPropertyValidator { }
