using FluentValidation;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Create;

public sealed class CreateSkillCategoryValidator : AbstractValidator<CreateSkillCategoryCommand>
{
    public CreateSkillCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}