using FluentValidation;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Update;

public sealed class UpdateJobPrioritizationValidator : AbstractValidator<UpdateJobPrioritizationCommand>
{
    public UpdateJobPrioritizationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.AuditableUnit)
            .NotEmpty()
            .WithMessage("AuditableUnit is required.")
            .MaximumLength(100)
            .WithMessage("AuditableUnit must not exceed 100 characters.");

        RuleFor(x => x.Comments)
            .MaximumLength(500)
            .WithMessage("Comments must not exceed 500 characters.");

        RuleFor(x => x.SelectedYear)
            .NotEmpty()
            .WithMessage("SelectedYear is required.")
            .Must(x => x.Year > 0)
            .WithMessage("SelectedYear must be a valid year.");

        RuleFor(x => x.BusinessObjectiveId)
            .NotEmpty()
            .WithMessage("BusinessObjectiveId is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("BusinessObjectiveId must not be empty.");

        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage("RatingId is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("RatingId must not be empty.");
    }
}