using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Update;

public class UpdateAuditEngagementValidator : AbstractValidator<UpdateAuditEngagementCommand>
{
    public UpdateAuditEngagementValidator()
    {
        RuleFor(x => x.JobName)
            .NotEmpty()
            .WithMessage("Job Name is required.")
            .MinimumLength(2)
            .WithMessage("Job Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Job Name must not exceed 200 characters.");

        RuleFor(x => x.PlannedStartDate)
            .NotEmpty()
            .WithMessage("Planned Start Date is required.");

        RuleFor(x => x.PlannedEndDate)
            .NotEmpty()
            .WithMessage("Planned End Date is required.");

        RuleFor(x => x.JobType)
            .NotEmpty()
            .WithMessage("Job Type is required.")
            .MinimumLength(2)
            .WithMessage("Job Type must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Job Type must not exceed 200 characters.");

        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("Location Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Location Id must be a valid GUID.");

        RuleFor(x => x.Status)
            .MaximumLength(50)
            .WithMessage("Status must not exceed 50 characters.");

        RuleFor(x => x.JobStatus)
            .MaximumLength(50)
            .WithMessage("Job Status must not exceed 50 characters.");
    }
}
