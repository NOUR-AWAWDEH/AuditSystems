using FluentValidation;

namespace AuditSystem.Application.Features.Skills.SkillSet.Update;

public sealed class UpdateSkillSetValidator : AbstractValidator<UpdateSkillSetCommand>
{
    public UpdateSkillSetValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");
        
        RuleFor(x => x.ProficiencyLevel)
            .NotEmpty()
            .WithMessage("ProficiencyLevel is required.")
            .MinimumLength(2)
            .WithMessage("ProficiencyLevel must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("ProficiencyLevel must not exceed 100 characters.");
    }
}