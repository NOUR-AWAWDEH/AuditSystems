using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Compliance;

internal sealed class ComplianceAuditLinkValidator<T> : PropertyValidator<T, string?>, IComplianceAuditLinkValidator
{
    public override string Name => "ComplianceAuditLinkValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Compliance audit link '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IComplianceAuditLinkValidator : IPropertyValidator { }
