using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Delete;

public sealed class DeleteAuditPlanSummaryValidator : AbstractValidator<DeleteAuditPlanSummaryCommand>
{
    public DeleteAuditPlanSummaryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Plan Summary Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Audit Plan Summary Id is not a valid GUID.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}