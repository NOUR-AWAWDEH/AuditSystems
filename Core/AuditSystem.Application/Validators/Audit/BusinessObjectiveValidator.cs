using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Audit;

internal sealed class BusinessObjectiveValidator<T> : PropertyValidator<T, string?>, IBusinessObjectiveValidator
{
    public override string Name => "BusinessObjectiveValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Business objective '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IBusinessObjectiveValidator : IPropertyValidator{}
