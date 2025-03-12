using AuditSystem.Application.Common.Validators;
using FluentValidation;

namespace AuditSystem.Application.Features.Risks.Commands.CreateRisk;

public sealed class CreateRiskValidator : AbstractValidator<CreateRiskCommand>
{
    public CreateRiskValidator()
    {
        RuleFor(x => x.RiskName)
            .ValidateRequiredString(3, 255, "Risk name");

        RuleFor(x => x.Description)
            .ValidateRequiredString(10, 5000, "Description");

        RuleFor(x => x.ObjectiveId)
            .ValidateRequiredGuid("Objective ID");

        RuleFor(x => x.Rating)
            .NotEmpty()
            .WithMessage("Rating is required.")
            .IsInEnum()
            .WithMessage("Rating must be a valid risk rating value.");
    }
}