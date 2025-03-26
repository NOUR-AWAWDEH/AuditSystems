using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Create;

public sealed class CreateRiskControlsValidator : AbstractValidator<CreateRiskControlsCommand>
{
    public CreateRiskControlsValidator()
    {
        RuleFor(x => x.RiskId)
            .NotEmpty()
            .WithMessage("Risk Id is required.");

        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage("Rating Id is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}
