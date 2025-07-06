using FluentValidation;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Delete;

public sealed class DeleteAuditExceptionReportValidator : AbstractValidator<DeleteAuditExceptionReportCommand>
{
    public DeleteAuditExceptionReportValidator()
    {
        RuleFor(c => c.Id)
        .NotEmpty()
        .WithMessage("Audit Exception Report Id Required.")
        .Must(IsValidGuid)
        .WithMessage("Audit Exception Report Id not Valid");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
