namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Delete;

using FluentValidation;

public sealed class DeleteInternalAuditConsolidationReportValidator : AbstractValidator<DeleteInternalAuditConsolidationReportCommand>
{
    public DeleteInternalAuditConsolidationReportValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("Internal Audit Consolidation Report Id is required")
        .Must(IsValidGuid)
        .WithMessage("Invalid Internal Audit Consolidation Report Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
