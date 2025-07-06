using FluentValidation;

namespace AuditSystem.Application.Features.Processes.SubProcess.Update;

public sealed class UpdateSubProcessValidator : AbstractValidator<UpdateSubProcessCommand>
{
    public UpdateSubProcessValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.Particular)
            .NotEmpty()
            .WithMessage("Particular is required.")
            .MinimumLength(2)
            .WithMessage("Particular must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Particular must not exceed 100 characters.");

        RuleFor(x => x.ProcessId)
            .NotEmpty()
            .WithMessage("ProcessId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("ProcessId must not be empty.");
    }
}
