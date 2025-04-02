using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Create;

public sealed class CreateRiskProgramValidator : AbstractValidator<CreateRiskProgramCommand>
{
    public CreateRiskProgramValidator() 
    {
        RuleFor(x => x.RiskControlId)
            .NotEmpty()
            .WithMessage("Risk Control Id is required.");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Risk Program Name is required.")
            .MaximumLength(200)
            .WithMessage("Risk Program Name must not exceed 200 characters.");

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