using FluentValidation;

namespace AuditSystem.Application.Features.Skills.Skill.Create;

public sealed class CreateSkillValidator : AbstractValidator<CreateSkillCommand>
{
    public CreateSkillValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Skill Name is required.")
            .MinimumLength(2)
            .WithMessage("Skill Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Skill Name must not exceed 200 characters.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Skill Category Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Skill Category Id must be a valid GUID.");

        RuleFor(x => x.SkillSetId)
            .NotEmpty()
            .WithMessage("Skill Set Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Skill Set Id must be a valid GUID.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Skill Description must not exceed 500 characters.");
    }
}
