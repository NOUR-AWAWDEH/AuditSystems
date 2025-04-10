using FluentValidation;
using AuditSystem.Application.Features.Jobs.AuditJob.Create;

public sealed class CreateAuditJobValidator : AbstractValidator<CreateAuditJobCommand>
{
    public CreateAuditJobValidator()
    {
        RuleFor(x => x.AuditUniverseID)
            .NotEmpty()
            .WithMessage("The AuditUniverseID field is required.");

        RuleFor(x => x.JobName)
            .NotEmpty()
            .WithMessage("The JobName field is required.")
            .MinimumLength(2)
            .WithMessage("The JobName field must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("The JobName field must not exceed 100 characters.");

        RuleFor(x => x.JobType)
            .NotEmpty()
            .WithMessage("The JobType field is required.")
            .MinimumLength(2)
            .WithMessage("The JobType field must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("The JobType field must not exceed 50 characters.");
    }
}