using FluentValidation;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Create;

public sealed class CreateRiskFactorValidator : AbstractValidator<CreateRiskFactorCommand>
{
    public CreateRiskFactorValidator() 
    {
        RuleFor(x => x.AuditorSettingsId)
            .NotEmpty()
            .WithMessage("Auditor Settings Id is required.");

        RuleFor(x => x.Factor)
            .NotEmpty()
            .WithMessage("Risk Factor Name is required.")
            .MaximumLength(200)
            .WithMessage("Risk Factor Name must not exceed 200 characters.");
    }
}
