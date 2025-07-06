using FluentValidation;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Get;

public sealed class GetPlanningReportByIdQueryValidator : AbstractValidator<GetPlanningReportByIdQuery>
{
    public GetPlanningReportByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Planning Report Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Planning Report Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
