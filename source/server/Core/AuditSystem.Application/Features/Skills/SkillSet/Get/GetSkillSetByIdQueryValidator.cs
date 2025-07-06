using FluentValidation;

namespace AuditSystem.Application.Features.Skills.SkillSet.Get;

public sealed class GetSkillSetByIdQueryValidator : AbstractValidator<GetSkillSetByIdQuery>
{
    public GetSkillSetByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Skill Set Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Skill Set Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty; 
    }    
}
