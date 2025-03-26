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
            .MaximumLength(100)
            .WithMessage("The JobName field must not exceed 100 characters.");

        RuleFor(x => x.JobType)
            .NotEmpty()
            .WithMessage("The JobType field is required.")
            .MaximumLength(50)
            .WithMessage("The JobType field must not exceed 50 characters.");
    }
}