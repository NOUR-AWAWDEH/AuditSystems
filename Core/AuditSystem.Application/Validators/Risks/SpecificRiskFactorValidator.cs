using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Risks;

internal sealed class SpecificRiskFactorValidator<T> : PropertyValidator<T, string?>, ISpecificRiskFactorValidator
{
    public override string Name => "SpecificRiskFactorValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "SpecificRiskFactor '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ISpecificRiskFactorValidator : IPropertyValidator { }
