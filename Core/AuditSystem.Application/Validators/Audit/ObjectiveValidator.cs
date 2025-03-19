using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Audit;

internal sealed class ObjectiveValidator<T> : PropertyValidator<T, string?>, IObjectiveValidator
{
    public override string Name => "ObjectiveValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Objective '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IObjectiveValidator : IPropertyValidator{}
