using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Skills;

internal sealed class SkillSetValidator<T> : PropertyValidator<T, string?>, ISkillSetValidator
{
    public override string Name => "SkillSetValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "SkillSet '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ISkillSetValidator : IPropertyValidator { }
