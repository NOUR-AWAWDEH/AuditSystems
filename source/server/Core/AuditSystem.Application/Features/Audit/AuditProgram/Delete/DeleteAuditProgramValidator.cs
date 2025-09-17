using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Delete;

public sealed class DeleteAuditProgramValidator : AbstractValidator<DeleteAuditProgramCommand>
{
    public DeleteAuditProgramValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Program ID is required.")
            .Must(IsValidGuid)
            .WithMessage("Audit Program ID must be a valid GUID.");
        
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
