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
            .MaximumLength(500)
            .WithMessage("Particular must not exceed 500 characters.");

        RuleFor(x => x.AuditProcessId)
            .NotEmpty()
            .WithMessage("AuditProcessId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("AuditProcessId must not be empty.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(BeAValidGuid)
            .WithMessage("UserId must not be empty.");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}