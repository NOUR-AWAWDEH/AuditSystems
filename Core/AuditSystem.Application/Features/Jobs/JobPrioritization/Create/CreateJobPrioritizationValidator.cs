using FluentValidation;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Create;

public sealed class CreateJobPrioritizationValidator : AbstractValidator<CreateJobPrioritizationCommand>
{
    public CreateJobPrioritizationValidator()
    {
        RuleFor(x => x.AuditableUnit)
            .NotEmpty()
            .WithMessage("Auditable Unit is required.")
            .MinimumLength(3)
            .WithMessage("Auditable Unit must be at least 3 characters long.")
            .MaximumLength(100)
            .WithMessage("Auditable Unit must not exceed 100 characters.");

        RuleFor(x => x.BusinessObjectiveId)
            .NotEmpty()
            .WithMessage("Business Objective ID is required.");

        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage("Rating ID is required.");

        RuleFor(x => x.Comments)
            .MaximumLength(500)
            .WithMessage("Comments must not exceed 500 characters.");

        RuleFor(x => x.SelectedYear)
            .NotEmpty()
            .WithMessage("Selected Year is required.")
            .Must(YearMustBeValid)
            .WithMessage("Selected Year must be a valid year.");
    }

    private bool YearMustBeValid(DateOnly date)
    {
        // Ensure the year is within a reasonable range (e.g., not in the future)
        return date.Year <= DateTime.Now.Year;
    }
}