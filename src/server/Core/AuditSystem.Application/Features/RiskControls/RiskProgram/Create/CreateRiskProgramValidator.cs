using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Create;

public sealed class CreateRiskProgramValidator : AbstractValidator<CreateRiskProgramCommand>
{
    public CreateRiskProgramValidator()
    {
        RuleFor(x => x.RiskControlId)
            .NotEmpty()
            .WithMessage("Risk Control Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Risk Control Id must be a valid GUID.");


        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Risk Program Name is required.")
            .MinimumLength(2)
            .WithMessage("Risk Program Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Risk Program Name must not exceed 200 characters.");

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