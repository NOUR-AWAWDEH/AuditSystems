using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.SupportingDocs;

internal sealed class SupportingDocValidator<T> : PropertyValidator<T, string?>, ISupportingDocValidator
{
    public override string Name => "SupportingDocValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "SupportingDoc '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ISupportingDocValidator : IPropertyValidator { }
