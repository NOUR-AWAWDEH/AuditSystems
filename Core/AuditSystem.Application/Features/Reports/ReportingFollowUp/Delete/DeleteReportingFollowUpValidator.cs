namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Delete;

using FluentValidation;

public sealed class DeleteReportingFollowUpValidator : AbstractValidator<DeleteReportingFollowUpCommand>
{
    public DeleteReportingFollowUpValidator()
    {
        RuleFor(x => x.Id)
       .NotEmpty()
       .WithMessage("Reporting Follow Up Id is required")
       .Must(IsValidGuid)
       .WithMessage("Invalid Reporting Follow Up Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
