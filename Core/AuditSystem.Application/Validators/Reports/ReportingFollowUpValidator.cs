using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Reports;

internal sealed class ReportingFollowUpValidator<T> : PropertyValidator<T, string?>, IReportingFollowUpValidator
{
    public override string Name => "ReportingFollowUpValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "ReportingFollowUp '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IReportingFollowUpValidator : IPropertyValidator { }
