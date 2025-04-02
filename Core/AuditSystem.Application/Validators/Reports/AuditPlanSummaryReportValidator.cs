using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Reports;

internal sealed class AuditPlanSummaryReportValidator<T> : PropertyValidator<T, string?>, IAuditPlanSummaryReportValidator
{
    public override string Name => "AuditPlanSummaryReportValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "AuditPlanSummaryReport '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IAuditPlanSummaryReportValidator : IPropertyValidator { }
