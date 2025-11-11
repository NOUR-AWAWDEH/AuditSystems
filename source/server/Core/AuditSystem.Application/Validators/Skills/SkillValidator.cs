using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Skills;

internal sealed class SkillValidator<T> : PropertyValidator<T, string?>, ISkillValidator
{
    public override string Name => "SkillValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Skill '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ISkillValidator : IPropertyValidator { }
