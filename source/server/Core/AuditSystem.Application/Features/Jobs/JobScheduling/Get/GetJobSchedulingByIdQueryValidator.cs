using FluentValidation;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Get;

public sealed class GetJobSchedulingByIdQueryValidator : AbstractValidator<GetJobSchedulingByIdQuery>
{
    public GetJobSchedulingByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Job Scheduling Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Job Scheduling Id must be a valid Guid");  
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}