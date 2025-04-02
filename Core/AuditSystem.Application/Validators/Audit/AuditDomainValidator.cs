using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Audit;

internal sealed class AuditDomainValidator<T> : PropertyValidator<T, string?>, IAuditDomainValidator
{
    public override string Name => "AuditDomainValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Audit domain '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IAuditDomainValidator : IPropertyValidator { }
