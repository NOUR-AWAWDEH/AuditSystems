using FluentValidation;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Delete;

public sealed class DeleteAuditProcessValidator : AbstractValidator<DeleteAuditProcessCommand>
{
    public DeleteAuditProcessValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Process id is Required")
            .Must(IsValidGuid)
            .WithMessage("Audit Process id is not valid");

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
