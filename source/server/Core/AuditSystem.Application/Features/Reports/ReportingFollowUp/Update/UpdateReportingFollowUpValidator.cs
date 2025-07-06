using FluentValidation;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Update;

public sealed class UpdateReportingFollowUpValidator : AbstractValidator<UpdateReportingFollowUpCommand>
{
    public UpdateReportingFollowUpValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.Reporting)
            .NotEmpty()
            .WithMessage("Reporting is required.")
            .MinimumLength(2)
            .WithMessage("Reporting must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Reporting must not exceed 100 characters.");

        RuleFor(x => x.FollowUp)
            .NotEmpty()
            .WithMessage("FollowUp is required.")
            .MinimumLength(2)
            .WithMessage("FollowUp must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("FollowUp must not exceed 100 characters.");

        RuleFor(x => x.Status)
            .MaximumLength(50)
            .WithMessage("Status must not exceed 50 characters.");
    }
}
