using FluentValidation;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Get;

public sealed class GetAuditJobByIdQueryValidator : AbstractValidator<GetAuditJobByIdQuery>
{
    public GetAuditJobByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Job Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Audit Job Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}