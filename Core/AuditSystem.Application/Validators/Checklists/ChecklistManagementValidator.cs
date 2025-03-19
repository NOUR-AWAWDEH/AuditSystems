using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Checklists;

internal sealed class ChecklistManagementValidator<T> : PropertyValidator<T, string?>, IChecklistManagementValidator
{
    public override string Name => "ChecklistManagementValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Checklist management '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IChecklistManagementValidator : IPropertyValidator{}
