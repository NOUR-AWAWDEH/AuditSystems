using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Domain.Entities.Skills;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
