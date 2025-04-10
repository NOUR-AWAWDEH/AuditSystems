using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Audit;

internal sealed class AuditPlanSummaryValidator<T> : PropertyValidator<T, string?>, IAuditPlanSummaryValidator
{
    public override string Name => "AuditPlanSummaryValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Audit plan summary '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IAuditPlanSummaryValidator : IPropertyValidator { }