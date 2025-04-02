using FluentValidation;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Create;

public sealed class CreateJobTimeAllocationReportValidator : AbstractValidator<CreateJobTimeAllocationReportCommand>
{
    public CreateJobTimeAllocationReportValidator() 
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

        RuleFor(x => x.CreatedById)
            .NotEmpty()
            .WithMessage("Created By Id is required.");

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
