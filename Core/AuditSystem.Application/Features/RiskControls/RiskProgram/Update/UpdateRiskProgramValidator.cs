using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Update;

public sealed class UpdateRiskProgramValidator : AbstractValidator<UpdateRiskProgramCommand>
{
    public UpdateRiskProgramValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.RiskControlId)
            .NotEmpty()
            .WithMessage("RiskControlId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("RiskControlId must not be empty.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

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