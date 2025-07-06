namespace AuditSystem.Application.Features.Skills.SkillSet.Delete;
using FluentValidation;
public sealed class DeleteSkillSetValidator : AbstractValidator<DeleteSkillSetCommand>
{
    public DeleteSkillSetValidator()
    {
        RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Skill Set Id is required")
          .Must(IsValidGuid)
          .WithMessage("Invalid Skill Set Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
