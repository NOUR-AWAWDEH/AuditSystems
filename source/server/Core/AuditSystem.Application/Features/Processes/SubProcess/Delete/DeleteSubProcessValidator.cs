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
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
