using FluentValidation;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Create;

public sealed class CreateReportingFollowUpValidator : AbstractValidator<CreateReportingFollowUpCommand>
{
    public CreateReportingFollowUpValidator()
    {
        RuleFor(x => x.Reporting)
            .NotEmpty()
            .WithMessage("Reporting is required.")
            .MinimumLength(2)
            .WithMessage("Reporting must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Reporting must not exceed 200 characters.");

        RuleFor(x => x.FollowUp)
            .NotEmpty()
            .WithMessage("Follow Up is required.")
            .MinimumLength(2)
            .WithMessage("Follow Up must be at least 2 characters long.")
            .MaximumLength(300)
            .WithMessage("Follow Up must not exceed 300 characters.");

        RuleFor(x => x.Status)
            .MaximumLength(300)
            .WithMessage("Status must not exceed 300 characters.");
    }
}
