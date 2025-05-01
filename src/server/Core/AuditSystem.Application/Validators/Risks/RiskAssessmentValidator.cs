using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Risks;

internal sealed class RiskAssessmentValidator<T> : PropertyValidator<T, string?>, IRiskAssessmentValidator
{
    public override string Name => "RiskAssessmentValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "RiskAssessment '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IRiskAssessmentValidator : IPropertyValidator { }
