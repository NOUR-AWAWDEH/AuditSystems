using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Create;

public sealed class CreateAuditUniverseObjectiveValidator : AbstractValidator<CreateAuditUniverseObjectiveCommand>
{
    public CreateAuditUniverseObjectiveValidator() 
    {
        RuleFor(x => x.AuditUniverseID)
            .NotEmpty()
            .WithMessage("Audit Universe ID is required");

        RuleFor(x => x.Impact)
            .NotEmpty()
            .WithMessage("Impact is required")
            .MaximumLength(200)
            .WithMessage("Impact cannot exceed 200 characters");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Amount must be greater than or equal to 0");

        RuleFor(x => x.ImpactAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Impact Amount must be greater than or equal to 0");

        RuleFor(x => x.Percentage)
            .InclusiveBetween(0, 100)
            .WithMessage("Percentage must be between 0 and 100");

    }
}