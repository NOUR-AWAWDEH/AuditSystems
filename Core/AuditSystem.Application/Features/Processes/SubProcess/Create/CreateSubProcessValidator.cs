using FluentValidation;

namespace AuditSystem.Application.Features.Processes.SubProcess.Create;

public sealed class CreateSubProcessValidator : AbstractValidator<CreateSubProcessCommand>
{
    public CreateSubProcessValidator() 
    {
        RuleFor(x => x.ProcessId)
            .NotEmpty()
            .WithMessage("Process Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Process Id must be a valid GUID.");

        RuleFor(x => x.Particular)
            .NotEmpty()
            .WithMessage("Particular is required.")
            .MinimumLength(2)
            .WithMessage("Particular must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Particular must not exceed 200 characters.");
    }
}
