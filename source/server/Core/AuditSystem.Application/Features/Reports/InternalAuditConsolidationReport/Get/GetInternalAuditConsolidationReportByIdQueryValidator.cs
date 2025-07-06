using FluentValidation;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Get;

public sealed class GetInternalAuditConsolidationReportByIdQueryValidator : AbstractValidator<GetInternalAuditConsolidationReportByIdQuery>
{
    public GetInternalAuditConsolidationReportByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Internal Audit Consolidation Report Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Internal Audit Consolidation Report Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
