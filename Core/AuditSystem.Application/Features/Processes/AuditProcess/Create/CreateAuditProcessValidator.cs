using FluentValidation;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Create;

public sealed class CreateAuditProcessValidator : AbstractValidator<CreateAuditProcessCommand>
{
    public CreateAuditProcessValidator()
    {
        
        RuleFor(x => x.ProcessName)
            .NotEmpty()
            .WithMessage("Audit Process Name is required.")
            .MinimumLength(2)
            .WithMessage("Audit Process Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Audit Process Name must not exceed 200 characters.");

        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage("Rating Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Rating Id must be a valid GUID.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Audit Process Description must not exceed 500 characters.");
    }
}