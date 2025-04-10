using FluentValidation;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Create;

public sealed class CreateAuditPlanSummaryReportValidator : AbstractValidator<CreateAuditPlanSummaryReportCommand>
{
    public CreateAuditPlanSummaryReportValidator() 
    {
        RuleFor(x => x.ReportName)
            .NotEmpty()
            .WithMessage("Report Name is required.")
            .MinimumLength(2)
            .WithMessage("Report Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Report Name must not exceed 200 characters.");

        RuleFor(x => x.ReportDate)
            .NotEmpty()
            .WithMessage("Report Date is required.")
            .Must(BeAValidDate)
            .WithMessage("Report Date must be a valid date.");

        RuleFor(x => x.CreatedById)
            .NotEmpty()
            .WithMessage("Created By Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Created By Id must be a valid GUID.");

        RuleFor(x => x.Status)
            .MinimumLength(2)
            .WithMessage("Status must be at least 2 characters long.")
            .MaximumLength(300)
            .WithMessage("Status must not exceed 300 characters.");

    }

    private bool BeAValidDate(DateOnly date)
    {
        return date != default && date <= DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
