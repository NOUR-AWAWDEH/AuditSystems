using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Organisation;

internal sealed class DepartmentValidator<T> : PropertyValidator<T, string?>, IDepartmentValidator
{
    public override string Name => "DepartmentValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Department '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IDepartmentValidator : IPropertyValidator { }
