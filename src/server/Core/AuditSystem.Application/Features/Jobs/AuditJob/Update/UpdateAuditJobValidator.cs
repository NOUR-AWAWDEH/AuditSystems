using FluentValidation;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Update;

public sealed class UpdateAuditJobValidator : AbstractValidator<UpdateAuditJobCommand>
{
    public UpdateAuditJobValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must not be empty.");
        
        RuleFor(x => x.JobName)
            .NotEmpty()
            .WithMessage("JobName is required.")
            .MaximumLength(100)
            .WithMessage("JobName must not exceed 100 characters.");
        
        RuleFor(x => x.JobType)
            .NotEmpty()
            .WithMessage("JobType is required.")
            .MaximumLength(100)
            .WithMessage("JobType must not exceed 100 characters.");
        
        RuleFor(x => x.AuditUniverseID)
            .NotEmpty()
            .WithMessage("AuditUniverseID is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("AuditUniverseID must not be empty.");
    }
}
