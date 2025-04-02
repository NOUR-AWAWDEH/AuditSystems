using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Processes;

internal sealed class SubProcessValidator<T> : PropertyValidator<T, string?>, ISubProcessValidator
{
    public override string Name => "SubProcessValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "SubProcess '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ISubProcessValidator : IPropertyValidator { }
