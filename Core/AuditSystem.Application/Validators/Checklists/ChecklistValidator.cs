using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Checklists;

internal sealed class ChecklistValidator<T> : PropertyValidator<T, string?>, IChecklistValidator
{
    public override string Name => "ChecklistValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Checklist '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IChecklistValidator : IPropertyValidator { }
