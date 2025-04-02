using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Jobs;

internal sealed class AuditJobValidator<T> : PropertyValidator<T, string?>, IAuditJobValidator
{
    public override string Name => "AuditJobValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Audit job '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IAuditJobValidator : IPropertyValidator { }
