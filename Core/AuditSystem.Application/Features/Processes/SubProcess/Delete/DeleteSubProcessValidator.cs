using FluentValidation;

namespace AuditSystem.Application.Features.Processes.SubProcess.Delete;

public sealed class DeleteSubProcessValidator :  AbstractValidator<DeleteSubProcessCommand>
{
    public DeleteSubProcessValidator() 
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("SubProcess Id is required.")
            .Must(IsValidGuid)
            .WithMessage("SubProcess Id is not valid");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
