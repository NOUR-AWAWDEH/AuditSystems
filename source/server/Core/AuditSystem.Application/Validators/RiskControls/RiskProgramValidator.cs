using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.RiskControls;

internal sealed class RiskProgramValidator<T> : PropertyValidator<T, string?>, IRiskProgramValidator
{
    public override string Name => "RiskProgramValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Risk Program '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IRiskProgramValidator : IPropertyValidator { }
