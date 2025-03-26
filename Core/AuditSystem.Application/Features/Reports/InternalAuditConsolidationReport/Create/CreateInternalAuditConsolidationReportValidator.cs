using FluentValidation;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Create;

public sealed class CreateInternalAuditConsolidationReportValidator : AbstractValidator<CreateInternalAuditConsolidationReportCommand>
{
    public CreateInternalAuditConsolidationReportValidator()
    {
        RuleFor(x => x.JobName)
            .NotEmpty()
            .WithMessage("Job Name is required.")
            .MaximumLength(200)
            .WithMessage("Job Name must not exceed 200 characters.");

        RuleFor(x => x.ReportName)
            .NotEmpty()
            .WithMessage("Report Name is required.")
            .MaximumLength(200)
            .WithMessage("Report Name must not exceed 200 characters.");

        RuleFor(x => x.ReportDate)
            .NotEmpty()
            .WithMessage("Report Date is required.")
            .Must(BeAValidDate)
            .WithMessage("Report Date must be a valid date.");

        RuleFor(x => x.PreparedByUserId)
            .NotEmpty()
            .WithMessage("Prepared By Id is required.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .MaximumLength(300)
            .WithMessage("Status must not exceed 300 characters.");
    }

    private bool BeAValidDate(DateOnly date)
    {
        return date != default;
    }
}