using FluentValidation;

namespace AuditSystem.Application.Features.Skills.SkillSet.Create;

public sealed class CreateSkillSetValidator : AbstractValidator<CreateSkillSetCommand>
{
    public CreateSkillSetValidator()
    {
        RuleFor(x => x.ProficiencyLevel)
            .NotEmpty()
            .WithMessage("Proficiency Level is required.")
            .MinimumLength(2)
            .WithMessage("Proficiency Level must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Proficiency Level must not exceed 200 characters.");
    }
}
