using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Delete;

public sealed class DeleteAuditDomainValidator : AbstractValidator<DeleteAuditDomainCommand>
{
    public DeleteAuditDomainValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Domain Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Audit Domain Id is not a valid GUID.");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}