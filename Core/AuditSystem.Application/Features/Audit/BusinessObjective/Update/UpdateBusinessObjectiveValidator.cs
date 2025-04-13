using FluentValidation;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Update;

public sealed class UpdateBusinessObjectiveValidator : AbstractValidator<UpdateBusinessObjectiveCommand>
{
    public UpdateBusinessObjectiveValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must be a valid GUID.");

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
    }
}

