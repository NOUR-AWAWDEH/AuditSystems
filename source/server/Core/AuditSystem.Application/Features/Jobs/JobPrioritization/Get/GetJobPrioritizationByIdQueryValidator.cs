using FluentValidation;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Get;

public sealed class GetJobPrioritizationByIdQueryValidator : AbstractValidator<GetJobPrioritizationByIdQuery>
{
    public GetJobPrioritizationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
          .WithMessage("Job Prioritization Id is required")
          .Must(BeAValidGuid)
          .WithMessage("Job Prioritization Id must be a valid Guid");    
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}