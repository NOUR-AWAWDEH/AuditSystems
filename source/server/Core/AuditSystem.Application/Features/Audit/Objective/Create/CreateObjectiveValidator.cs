using FluentValidation;

namespace AuditSystem.Application.Features.Audit.Objective.Create;

public sealed class CreateObjectiveValidator : AbstractValidator<CreateObjectiveCommand>
{
    public CreateObjectiveValidator()
    {

        RuleFor(x => x.RiskControlMatrixId)
            .NotEmpty()
            .WithMessage("Risk Control Matrix ID is required.");

        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage("Rating ID is required.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}