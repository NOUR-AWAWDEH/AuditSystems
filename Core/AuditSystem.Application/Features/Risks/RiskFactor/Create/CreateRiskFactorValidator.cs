using FluentValidation;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Create;

public sealed class CreateRiskFactorValidator : AbstractValidator<CreateRiskFactorCommand>
{
    public CreateRiskFactorValidator() 
    {
       
        RuleFor(x => x.Factor)
            .NotEmpty()
            .WithMessage("Risk Factor Name is required.")
            .MinimumLength(2)
            .WithMessage("Risk Factor Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Risk Factor Name must not exceed 200 characters.");
    }
}
