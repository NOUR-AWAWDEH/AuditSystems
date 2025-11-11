using FluentValidation;

namespace AuditSystem.Application.Features.Observation.Delete;

public sealed class DeleteObservationValidator : AbstractValidator<DeleteObservationCommand>
{
    public DeleteObservationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Observation ID is required.")
            .Must(IsValidGuid)
            .WithMessage("Observation ID must be a valid GUID.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(IsValidGuid)
            .WithMessage("User Id must be a valid GUID.");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
