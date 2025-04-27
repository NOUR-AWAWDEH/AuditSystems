using FluentValidation;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Delete;

public sealed class DeleteAuditJobValidator : AbstractValidator<DeleteAuditJobCommand>
{
    public DeleteAuditJobValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Job Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Audit Job Id cannot be empty.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}
