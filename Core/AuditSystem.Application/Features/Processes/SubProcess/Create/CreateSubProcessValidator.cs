using FluentValidation;

namespace AuditSystem.Application.Features.Processes.SubProcess.Create;

public sealed class CreateSubProcessValidator : AbstractValidator<CreateSubProcessCommand>
{
    public CreateSubProcessValidator() 
    {
        RuleFor(x => x.ProcessId)
            .NotEmpty()
            .WithMessage("Process Id is required.");

        RuleFor(x => x.Particular)
            .NotEmpty()
            .WithMessage("Particular is required.")
            .MaximumLength(200)
            .WithMessage("Particular must not exceed 200 characters.");
    }
}
