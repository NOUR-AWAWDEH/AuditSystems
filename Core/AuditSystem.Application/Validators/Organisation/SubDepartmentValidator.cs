using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Organisation;

internal sealed class SubDepartmentValidator<T> : PropertyValidator<T, string?>, ISubDepartmentValidator
{
    public override string Name => "SubDepartmentValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "SubDepartment '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ISubDepartmentValidator : IPropertyValidator { }
