using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Skills;

internal sealed class SkillCategoryValidator<T> : PropertyValidator<T, string?>, ISkillCategoryValidator
{
    public override string Name => "SkillCategoryValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
         => "SkillCategory '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ISkillCategoryValidator : IPropertyValidator { }
