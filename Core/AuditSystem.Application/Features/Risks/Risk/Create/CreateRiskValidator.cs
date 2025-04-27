using AuditSystem.Application.Common.Validators;
using FluentValidation;

namespace AuditSystem.Application.Features.Risks.Risk.Create;

public sealed class CreateRiskValidator : AbstractValidator<CreateRiskCommand>
{
    public CreateRiskValidator()
    {
        RuleFor(x => x.RiskName)
            .ValidateRequiredString(2, 255, "Risk name");

        RuleFor(x => x.RatingId)
           .NotEmpty()
           .WithMessage("Rating is required.")
           .Must(x => x != Guid.Empty)
           .WithMessage("Rating is required.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.ObjectiveId)
            .NotEmpty()
            .WithMessage("Objective ID is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Objective ID is required.");
    }
}