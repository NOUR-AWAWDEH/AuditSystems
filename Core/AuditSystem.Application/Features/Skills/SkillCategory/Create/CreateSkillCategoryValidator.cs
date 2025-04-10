using FluentValidation;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Create;

public sealed class CreateSkillCategoryValidator : AbstractValidator<CreateSkillCategoryCommand>
{
    public CreateSkillCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}