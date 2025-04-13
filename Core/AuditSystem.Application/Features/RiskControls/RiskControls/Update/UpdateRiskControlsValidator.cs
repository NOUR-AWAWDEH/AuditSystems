using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Update;

public sealed class UpdateRiskControlsValidator : AbstractValidator<UpdateRiskControlsCommand>
{
    public UpdateRiskControlsValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");
        
        RuleFor(x => x.RiskId)
            .NotEmpty()
            .WithMessage("RiskId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("RiskId must not be empty.");
        
        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage("RatingId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("RatingId must not be empty.");
        
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}