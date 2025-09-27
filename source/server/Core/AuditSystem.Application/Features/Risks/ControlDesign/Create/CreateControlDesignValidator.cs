using AuditSystem.Application.Common.Validators;
using FluentValidation;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Create;

public sealed class CreateControlDesignValidator : AbstractValidator<CreateControlDesignCommand>
{
    public CreateControlDesignValidator()
    {
        RuleFor(x => x.Description)
            .ValidateRequiredString(2, 500, "Description");

        RuleFor(x => x.ExitControlId)
           .NotEmpty()
           .WithMessage("Exit Control ID is required.")
           .Must(x => x != Guid.Empty)
           .WithMessage("Exit Control ID is required.");


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