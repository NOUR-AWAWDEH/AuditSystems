using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.Objective.Delete;

public sealed class DeleteObjectiveValidator : AbstractValidator<DeleteObjectiveCommand>
{
    public DeleteObjectiveValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Objective Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Objective Id is not a valid GUID.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(IsValidGuid)
            .WithMessage("User Id is not a valid GUID.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}