using FluentValidation;

namespace AuditSystem.Application.Features.Users.SkillSet.Create;

public sealed class CreateSkillSetValidator : AbstractValidator<CreateSkillSetCommand>
{
    public CreateSkillSetValidator() 
    {
        RuleFor(x => x.UserManagementId)
            .NotEmpty()
            .WithMessage("User Management Id is required.");

        RuleFor(x => x.SkillId)
            .NotEmpty()
            .WithMessage("Skill Id is required.");

        RuleFor(x => x.ProficiencyLevel)
            .NotEmpty()
            .WithMessage("Proficiency Level is required.")
            .MaximumLength(200)
            .WithMessage("Proficiency Level must not exceed 200 characters.");
    }
}
