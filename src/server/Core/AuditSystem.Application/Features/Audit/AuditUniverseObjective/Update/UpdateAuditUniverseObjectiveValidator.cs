using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Update;

public class UpdateAuditUniverseObjectiveValidator : AbstractValidator<UpdateAuditUniverseObjectiveCommand>
{
    public UpdateAuditUniverseObjectiveValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must be a valid GUID.");

        RuleFor(x => x.AuditUniverseID)
            .NotEmpty()
            .WithMessage("Audit Universe ID is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Audit Universe ID must be a valid GUID.");

        RuleFor(x => x.Impact)
            .NotEmpty()
            .WithMessage("Impact is required.")
            .MinimumLength(2)
            .WithMessage("Impact must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Impact must not exceed 200 characters.");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount is required.")
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.ImpactAmount)
            .NotEmpty()
            .WithMessage("Impact Amount is required.")
            .GreaterThan(0)
            .WithMessage("Impact Amount must be greater than 0.");

        RuleFor(x => x.Percentage)
            .NotEmpty()
            .WithMessage("Percentage is required.")
            .InclusiveBetween(0, 100)
            .WithMessage("Percentage must be between 0 and 100.");
    }
}
