using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Audit;

internal sealed class AuditUniverseValidator<T> : PropertyValidator<T, string?>, IAuditUniverseValidator
{
    public override string Name => "AuditUniverseValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Audit universe '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IAuditUniverseValidator : IPropertyValidator { }
