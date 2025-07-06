using FluentValidation;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Update;

public sealed class UpdatePlanningReportValidator : AbstractValidator<UpdatePlanningReportCommand>
{
    public UpdatePlanningReportValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.ReportName)
            .NotEmpty()
            .WithMessage("ReportName is required.")
            .MinimumLength(2)
            .WithMessage("ReportName must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("ReportName must not exceed 100 characters.");

        RuleFor(x => x.ReportDate)
            .NotEmpty()
            .WithMessage("ReportDate is required.")
            .Must(date => IsValidDate(date))
            .WithMessage("ReportDate must be a valid date.");

        RuleFor(x => x.CreatedById)
            .NotEmpty()
            .WithMessage("CreatedById is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("CreatedById must not be empty.");

        RuleFor(x => x.Status)
            .MaximumLength(50)
            .WithMessage("Status must not exceed 50 characters.");
    }
    public bool IsValidDate(DateOnly date)
    {
        return date != default(DateOnly);
    }
}
