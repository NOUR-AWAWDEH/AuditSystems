using FluentValidation;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Create;

public sealed class CreateBusinessObjectiveValidator : AbstractValidator<CreateBusinessObjectiveCommand>
{
    public CreateBusinessObjectiveValidator()
    {
        RuleFor(x => x.AuditorSettingsId)
            .NotEmpty()
            .WithMessage("Auditor Settings Id is required");

        RuleFor(x => x.Impact)
            .NotEmpty()
            .WithMessage("Impact is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");
    }
}
