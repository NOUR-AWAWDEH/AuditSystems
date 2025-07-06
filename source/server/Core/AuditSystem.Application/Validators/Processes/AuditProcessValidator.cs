using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Processes;

internal sealed class AuditProcessValidator<T> : PropertyValidator<T, string?>, IAuditProcessValidator
{
    public override string Name => "AuditProcessValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "AuditProcess '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IAuditProcessValidator : IPropertyValidator { }
