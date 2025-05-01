using FluentValidation;

namespace AuditSystem.Application.Features.Skills.Skill.Get;

public sealed class GetSkillByIdQueryValidator : AbstractValidator<GetSkillByIdQuery>
{
    public GetSkillByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Skill Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Skill Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}