using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Organisation;

internal sealed class SubLocationValidator<T> : PropertyValidator<T, string?>, ISubLocationValidator
{
    public override string Name => "SubLocationValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "SubLocation '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ISubLocationValidator : IPropertyValidator { }
