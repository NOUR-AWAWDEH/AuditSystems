using AuditSystem.Application.Features.Jobs.JobScheduling.Create;
using FluentValidation;

public sealed class CreateJobSchedulingValidator : AbstractValidator<CreateJobSchedulingCommand>
{
    public CreateJobSchedulingValidator()
    {
        RuleFor(x => x.AuditableUnit)
            .NotEmpty()
            .WithMessage("Auditable Unit is required.")
            .MinimumLength(2)
            .WithMessage("Auditable Unit must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Auditable Unit must not exceed 100 characters.");

        RuleFor(x => x.AuditYear)
            .NotEmpty()
            .WithMessage("Audit Year is required.")
            .Must(YearMustBeValid)
            .WithMessage("Audit Year must be a valid year.");

        RuleFor(x => x.PlannedStartDate)
            .NotEmpty()
            .WithMessage("Planned Start Date is required.")
            .Must(DateMustBeInFuture)
            .WithMessage("Planned Start Date must be in the future.");

        RuleFor(x => x.PlannedEndDate)
            .NotEmpty()
            .WithMessage("Planned End Date is required.")
            .Must(DateMustBeInFuture)
            .WithMessage("Planned End Date must be in the future.")
            .Must((command, endDate) => endDate >= command.PlannedStartDate)
            .WithMessage("Planned End Date must be after Planned Start Date.");

        RuleFor(x => x.Status)
            .MinimumLength(2)
            .WithMessage("Status must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("Status must not exceed 50 characters.");
    }

    private bool YearMustBeValid(DateOnly date)
    {
        return date.Year <= DateTime.Now.Year;
    }


    private bool DateMustBeInFuture(DateOnly date)
    {
        return date > DateOnly.FromDateTime(DateTime.Now);
    }
}