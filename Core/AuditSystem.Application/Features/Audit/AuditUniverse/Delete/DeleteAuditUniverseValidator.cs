using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Delete;

public sealed class DeleteAuditUniverseValidator : AbstractValidator<DeleteAuditUniverseCommand>
{
    public DeleteAuditUniverseValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Universe Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Audit Universe Id is not a valid GUID.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}
