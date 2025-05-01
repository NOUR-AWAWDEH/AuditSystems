using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Create;
public sealed class CreateAuditEngagementValidator : AbstractValidator<CreateAuditEngagementCommand>
{
    public CreateAuditEngagementValidator()
    {
        RuleFor(x => x.JobName)
            .NotEmpty()
            .WithMessage("The Job name field is required.")
            .MinimumLength(3)
            .WithMessage("The Job name must be at least 3 characters long.")
            .MaximumLength(255)
            .WithMessage("The Job name must not exceed 255 characters.");

        RuleFor(x => x.PlannedStartDate)
           .NotEmpty()
           .WithMessage("Planned Start Date is required.");


        RuleFor(x => x.PlannedEndDate)
            .NotEmpty()
            .WithMessage("Planned End Date is required.")
            .Must(DateMustBeInFuture)
            .WithMessage("Planned End Date must be in the future.")
            .Must((command, endDate) => endDate >= command.PlannedStartDate)
            .WithMessage("Planned End Date must be after Planned Start Date.");

        RuleFor(x => x.JobType)
            .NotEmpty()
            .WithMessage("The Job type field is required.")
            .MinimumLength(3)
            .WithMessage("The Job type must be at least 3 characters long.")
            .MaximumLength(255)
            .WithMessage("The Job type must not exceed 255 characters.");

        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("The Location ID field is required.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("The Status field is required.")
            .MinimumLength(3)
            .WithMessage("The Status must be at least 3 characters long.")
            .MaximumLength(255)
            .WithMessage("The Status must not exceed 255 characters.");

        RuleFor(x => x.JobStatus)
            .NotEmpty()
            .WithMessage("The Job status field is required.")
            .MinimumLength(3)
            .WithMessage("The Job status must be at least 3 characters long.")
            .MaximumLength(255)
            .WithMessage("The Job status must not exceed 255 characters.")
            .IsInEnum()
            .WithMessage("Job status must be a valid job status value.");
    }

    private bool DateMustBeInFuture(DateOnly date)
    {
        return date > DateOnly.FromDateTime(DateTime.Now);
    }
}