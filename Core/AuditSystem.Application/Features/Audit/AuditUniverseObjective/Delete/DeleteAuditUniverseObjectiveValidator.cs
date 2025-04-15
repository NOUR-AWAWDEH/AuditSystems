using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Delete;

public sealed class DeleteAuditUniverseObjectiveValidator : AbstractValidator<DeleteAuditUniverseObjectiveCommand>
{
    public DeleteAuditUniverseObjectiveValidator()
    {
        RuleFor(x => x.Id)
             .NotEmpty()
             .WithMessage("Audit Universe Objective Id is required.")
             .Must(IsValidGuid)
             .WithMessage("Audit Universe Objective Id is not a valid GUID.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}