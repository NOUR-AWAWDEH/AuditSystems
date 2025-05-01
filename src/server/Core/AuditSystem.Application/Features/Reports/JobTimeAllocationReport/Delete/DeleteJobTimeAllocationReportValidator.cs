namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Delete;

using FluentValidation;

public sealed class DeleteJobTimeAllocationReportValidator : AbstractValidator<DeleteJobTimeAllocationReportCommand>
{
    public DeleteJobTimeAllocationReportValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
           .WithMessage("Job Time Allocation Report Id is required")
          .Must(IsValidGuid)
          .WithMessage("Invalid Job Time Allocation Report Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
