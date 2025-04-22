using FluentValidation;

namespace AuditSystem.Application.Features.Audit.Objective.Get;

public sealed class GetObjectiveByIdQueryValidator : AbstractValidator<GetObjectiveByIdQuery>
{
    public GetObjectiveByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Objective Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Objective Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;  
    }
}
