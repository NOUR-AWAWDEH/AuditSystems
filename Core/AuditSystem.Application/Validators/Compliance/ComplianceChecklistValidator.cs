using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Compliance;

internal sealed class ComplianceChecklistValidator<T> : PropertyValidator<T, string?>, IComplianceChecklistValidator
{
    public override string Name => "ComplianceChecklistValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Compliance checklist '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IComplianceChecklistValidator : IPropertyValidator { }
