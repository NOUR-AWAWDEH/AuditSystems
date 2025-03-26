using FluentValidation;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Create;

public sealed class CreateAuditProcessValidator : AbstractValidator<CreateAuditProcessCommand>
{
    public CreateAuditProcessValidator()
    {
        RuleFor(x => x.AuditSettingsId)
            .NotEmpty()
            .WithMessage("Audit Settings Id is required.");

        RuleFor(x => x.ProcessName)
            .NotEmpty()
            .WithMessage("Audit Process Name is required.")
            .MaximumLength(200)
            .WithMessage("Audit Process Name must not exceed 200 characters.");

        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage("Rating Id is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Audit Process Description is required.")
            .MaximumLength(500)
            .WithMessage("Audit Process Description must not exceed 500 characters.");
    }
}