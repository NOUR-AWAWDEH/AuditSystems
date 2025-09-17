using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.Objective.Create;

public sealed class CreateObjectiveValidator : AbstractValidator<CreateObjectiveCommand>
{
    public CreateObjectiveValidator()
    {

        RuleFor(x => x.RiskControlMatrixId)
            .NotEmpty()
            .WithMessage("Risk Control Matrix ID is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Risk Control Matrix Id must be a valid GUID.");


        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");
    }
}