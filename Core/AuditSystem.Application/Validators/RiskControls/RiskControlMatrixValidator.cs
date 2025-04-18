using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.RiskControls;

internal sealed class RiskControlMatrixValidator<T> : PropertyValidator<T, string?>, IRiskControlMatrixValidator
{
    public override string Name => "RiskControlMatrixValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "RiskControlMatrix '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IRiskControlMatrixValidator : IPropertyValidator { }
