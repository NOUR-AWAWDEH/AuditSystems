using FluentValidation;

namespace AuditSystem.Application.Features.Users.Skill.Create;

public sealed class CreateSkillValidator : AbstractValidator<CreateSkillCommand>
{
    public CreateSkillValidator() 
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Skill Name is required.")
            .MaximumLength(200)
            .WithMessage("Skill Name must not exceed 200 characters.");
        
        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Skill Category is required.")
            .MaximumLength(200)
            .WithMessage("Skill Category must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Skill Description is required.")
            .MaximumLength(500)
            .WithMessage("Skill Description must not exceed 500 characters.");
    }
}
