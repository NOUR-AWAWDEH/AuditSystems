using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Checklists;

internal sealed class RemarkValidator<T> : PropertyValidator<T, string?>, IRemarkValidator
{
    public override string Name => "RemarkValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Remark '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IRemarkValidator : IPropertyValidator{}
