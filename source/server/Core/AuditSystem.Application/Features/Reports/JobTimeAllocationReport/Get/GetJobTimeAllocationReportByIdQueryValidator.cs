using FluentValidation;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Get;

public sealed class GetJobTimeAllocationReportByIdQueryValidator : AbstractValidator<GetJobTimeAllocationReportByIdQuery>
{
    public GetJobTimeAllocationReportByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Job Time Allocation Report Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Job Time Allocation Report Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }    
}
