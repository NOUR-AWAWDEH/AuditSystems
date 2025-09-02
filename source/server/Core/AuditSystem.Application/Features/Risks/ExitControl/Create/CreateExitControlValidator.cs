using AuditSystem.Application.Common.Validators;
using FluentValidation;

namespace AuditSystem.Application.Features.Risks.ExitControl.Create;

public sealed class CreateExitControlValidator : AbstractValidator<CreateExitControlCommand>
{
    public CreateExitControlValidator()
    {
        RuleFor(x => x.Description)
            .ValidateRequiredString(2, 500, "Description");

        RuleFor(x => x.RiskId)
           .NotEmpty()
           .WithMessage("Rate is required.")
           .Must(x => x != Guid.Empty)
           .WithMessage("Rate is required.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Inadequate)
            .NotNull()
            .WithMessage("Inadequate is required.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User ID is required.");
    }
}