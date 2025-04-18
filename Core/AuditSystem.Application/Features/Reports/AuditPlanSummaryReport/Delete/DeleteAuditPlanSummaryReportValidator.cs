using FluentValidation;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Delete;

public sealed class DeleteAuditPlanSummaryReportValidator : AbstractValidator<DeleteAuditPlanSummaryReportCommand>
{
    public DeleteAuditPlanSummaryReportValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Audit PlanSummary Report Id Requierd")
            .Must(IsValidGuid)
            .WithMessage("");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
