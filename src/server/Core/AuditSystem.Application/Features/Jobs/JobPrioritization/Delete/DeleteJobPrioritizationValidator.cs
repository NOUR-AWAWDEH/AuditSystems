using FluentValidation;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Delete;

public sealed class DeleteJobPrioritizationValidator : AbstractValidator<DeleteJobPrioritizationCommand>
{
    public DeleteJobPrioritizationValidator() 
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Job Prioritization Id is required.")
           .Must(IsValidGuid)
           .WithMessage("Job Prioritization Id cannot be empty.");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
