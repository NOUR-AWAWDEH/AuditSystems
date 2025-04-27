using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Get;

public sealed class GetAuditUniverseByIdQueryValidator : AbstractValidator<GetAuditUniverseByIdQuery>
{
    public GetAuditUniverseByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Universe Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Audit Universe Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}