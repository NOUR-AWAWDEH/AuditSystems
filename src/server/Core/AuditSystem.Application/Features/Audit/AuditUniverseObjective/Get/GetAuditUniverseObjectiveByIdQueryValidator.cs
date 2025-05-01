using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Get;

public sealed class GetAuditUniverseObjectiveByIdQueryValidator : AbstractValidator<GetAuditUniverseObjectiveByIdQuery>
{
    public GetAuditUniverseObjectiveByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Universe Objective Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Audit Universe Objective Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}