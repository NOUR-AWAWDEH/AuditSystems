using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Create;

public sealed class CreateAuditProgramCommandValidator : AbstractValidator<CreateAuditProgramCommand>
{
    public CreateAuditProgramCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Audit Program Description is required.")
            .MinimumLength(2)
            .WithMessage("Audit Program Description must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Audit Program Description must not exceed 500 characters.");
        
        RuleFor(x => x.ExitControlId)
            .NotEmpty()
            .WithMessage("Exit Control Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Exit Control Id must be a valid GUID.");
    }
}
