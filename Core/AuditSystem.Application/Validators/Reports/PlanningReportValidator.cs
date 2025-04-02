using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Reports;

internal sealed class PlanningReportValidator<T> : PropertyValidator<T, string?>, IPlanningReportValidator
{
    public override string Name => "PlanningReportValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "PlanningReport '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IPlanningReportValidator : IPropertyValidator { }
