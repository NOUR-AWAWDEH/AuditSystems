namespace AuditSystem.Application.Features.Skills.Skill.Delete;
using FluentValidation;

public sealed class DeleteSkillValidator : AbstractValidator<DeleteSkillCommand>
{
    public DeleteSkillValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Skill Id is required")
           .Must(IsValidGuid)
           .WithMessage("Invalid Skill Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
