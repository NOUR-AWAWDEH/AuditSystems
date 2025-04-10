using FluentValidation;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Create;

public sealed class CreateBusinessObjectiveValidator : AbstractValidator<CreateBusinessObjectiveCommand>
{
    public CreateBusinessObjectiveValidator()
    {
        RuleFor(x => x.Impact)
            .NotEmpty()
            .WithMessage("Impact is required")
            .MinimumLength(2)
            .WithMessage("Impact must be at least 2 characters long")
            .MaximumLength(255)
            .WithMessage("Impact cannot exceed 255 characters");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount is required")
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");
    }
}
