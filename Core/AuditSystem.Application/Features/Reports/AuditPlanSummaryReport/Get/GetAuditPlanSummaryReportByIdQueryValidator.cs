using FluentValidation;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Get;

public sealed class GetAuditPlanSummaryReportByIdQueryValidator : AbstractValidator<GetAuditPlanSummaryReportByIdQuery>
{
    public GetAuditPlanSummaryReportByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Plan Summary Report Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Audit Plan Summary Report Id must be a valid Guid");  
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
