using FluentValidation;

namespace AuditSystem.Application.Features.Risks.Risk.Update;

public sealed class UpdateRiskValidator : AbstractValidator<UpdateRiskCommand>
{
    public UpdateRiskValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");
        
        RuleFor(x => x.ObjectiveId)
            .NotEmpty()
            .WithMessage("ObjectiveId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("ObjectiveId must not be empty.");
        
        RuleFor(x => x.RiskName)
            .NotEmpty()
            .WithMessage("RiskName is required.")
            .MinimumLength(2)
            .WithMessage("RiskName must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("RiskName must not exceed 100 characters.");
        
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