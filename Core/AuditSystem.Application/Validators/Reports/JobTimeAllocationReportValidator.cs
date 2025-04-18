using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Reports;

internal sealed class JobTimeAllocationReportValidator<T> : PropertyValidator<T, string?>, IJobTimeAllocationReportValidator
{
    public override string Name => "JobTimeAllocationReportValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "JobTimeAllocationReport '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IJobTimeAllocationReportValidator : IPropertyValidator { }
