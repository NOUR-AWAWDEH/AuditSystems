namespace AuditSystem.Application.Features.Skills.SkillCategory.Delete;
using FluentValidation;

public sealed class DeleteSkillCategoryValidator : AbstractValidator<DeleteSkillCategoryCommand>
{
    public DeleteSkillCategoryValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Skill Category Id is required")
           .Must(IsValidGuid)
           .WithMessage("Invalid Skill Category Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
