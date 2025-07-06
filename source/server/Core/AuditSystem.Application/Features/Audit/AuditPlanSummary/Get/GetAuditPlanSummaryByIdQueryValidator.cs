using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Get;

public sealed class GetAuditPlanSummaryByIdQueryValidator : AbstractValidator<GetAuditPlanSummaryByIdQuery>
{
    public GetAuditPlanSummaryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Plan Summary Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Audit Plan Summary Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }    
}