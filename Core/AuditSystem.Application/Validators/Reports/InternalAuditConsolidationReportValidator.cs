using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Reports;

internal sealed class InternalAuditConsolidationReportValidator<T> : PropertyValidator<T, string?>, IInternalAuditConsolidationReportValidator
{
    public override string Name => "InternalAuditConsolidationReportValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "InternalAuditConsolidationReport '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IInternalAuditConsolidationReportValidator : IPropertyValidator { }
