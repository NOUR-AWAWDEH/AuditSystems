using FluentValidation;

namespace AuditSystem.Application.Features.Audit.Objective.Update;

public sealed class UpdateObjectiveValidator : AbstractValidator<UpdateObjectiveCommand>
{
    public UpdateObjectiveValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must be a valid GUID.");


        RuleFor(x => x.RiskControlMatrixId)
            .NotEmpty()
            .WithMessage("RiskControlMatrixId is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("RiskControlMatrixId must be a valid GUID.");


        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage("RatingId is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("RatingId must be a valid GUID.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}
