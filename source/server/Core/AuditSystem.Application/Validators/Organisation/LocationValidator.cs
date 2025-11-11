using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Organisation;

internal sealed class LocationValidator<T> : PropertyValidator<T, string?>, ILocationValidator
{
    public override string Name => "LocationValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Location '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ILocationValidator : IPropertyValidator { }
