namespace AuditSystem.Application.Features.Reports.PlanningReport.Delete;

using FluentValidation;

public sealed class DeletePlanningReportValidator : AbstractValidator<DeletePlanningReportCommand>
{
    public DeletePlanningReportValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Planning Report Id is required")
            .Must(IsValidGuid)
            .WithMessage("Invalid Planning Report Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
