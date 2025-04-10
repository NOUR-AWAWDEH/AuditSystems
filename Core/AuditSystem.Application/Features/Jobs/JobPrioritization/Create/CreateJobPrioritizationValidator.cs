using FluentValidation;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Create;

public sealed class CreateJobPrioritizationValidator : AbstractValidator<CreateJobPrioritizationCommand>
{
    public CreateJobPrioritizationValidator()
    {
        RuleFor(x => x.AuditableUnit)
            .NotEmpty()
            .WithMessage("Auditable Unit is required.")
            .MinimumLength(2)
            .WithMessage("Auditable Unit must be at least 2 characters long.")
            .MaximumLength(255)
            .WithMessage("Auditable Unit must not exceed 255 characters.");

        RuleFor(x => x.SelectForAudit)
            .NotEmpty()
            .WithMessage("Select For Audit is required.")
            .Must(x => x == true || x == false)
            .WithMessage("Select For Audit must be either true or false.");

        RuleFor(x => x.Comments)
            .MaximumLength(500)
            .WithMessage("Comments must not exceed 500 characters.");

        RuleFor(x => x.SelectedYear)
            .NotEmpty()
            .WithMessage("Selected Year is required.")
            .Must(YearMustBeValid)
            .WithMessage("Selected Year must be a valid year.");

        RuleFor(x => x.BusinessObjectiveId)
            .NotEmpty()
            .WithMessage("Business Objective Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Business Objective Id must be a valid GUID.");


        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage("Rating Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Rating Id must be a valid GUID.");

    }

    private bool YearMustBeValid(DateOnly date)
    {
        return date.Year <= DateTime.Now.Year;
    }
}