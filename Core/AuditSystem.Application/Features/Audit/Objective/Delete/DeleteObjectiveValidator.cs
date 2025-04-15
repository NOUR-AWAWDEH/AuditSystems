using FluentValidation;

namespace AuditSystem.Application.Features.Audit.Objective.Delete;

public sealed class DeleteObjectiveValidator : AbstractValidator<DeleteObjectiveCommand>
{
    public DeleteObjectiveValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Objective Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Objective Id is not a valid GUID.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}