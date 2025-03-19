using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Reports;

internal sealed class AuditExceptionReportValidator<T> : PropertyValidator<T, string?>, IAuditExceptionReportValidator
{
    public override string Name => "AuditExceptionReportValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "AuditExceptionReport '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IAuditExceptionReportValidator : IPropertyValidator{}
