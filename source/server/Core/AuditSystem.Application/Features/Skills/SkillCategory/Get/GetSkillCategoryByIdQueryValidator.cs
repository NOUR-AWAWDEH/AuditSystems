using FluentValidation;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Get;

public sealed class GetSkillCategoryByIdQueryValidator : AbstractValidator<GetSkillCategoryByIdQuery>
{
    public GetSkillCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Skill Category Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Skill Category Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;  
    }
}