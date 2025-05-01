using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Create;

public sealed class CreateRiskControlsValidator : AbstractValidator<CreateRiskControlsCommand>
{
    public CreateRiskControlsValidator()
    {
        RuleFor(x => x.RiskId)
            .NotEmpty()
            .WithMessage("Risk Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Risk Id must be a valid GUID.");

        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage("Rating Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Rating Id must be a valid GUID.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}
