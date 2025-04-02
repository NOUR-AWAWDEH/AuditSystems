using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Audit;

internal sealed class AuditEngagementValidator<T> : PropertyValidator<T, string?>, IAuditEngagementValidator
{
    public override string Name => "AuditEngagementValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Audit engagement '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IAuditEngagementValidator : IPropertyValidator { }
