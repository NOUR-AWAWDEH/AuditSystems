using FluentValidation;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Update;

public sealed class UpdateRiskFactorValidator : AbstractValidator<UpdateRiskFactorCommand>
{
    public UpdateRiskFactorValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.Factor)
            .NotEmpty()
            .WithMessage("Factor is required.")
            .MinimumLength(2)
            .WithMessage("Factor must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Factor must not exceed 100 characters.");
    }
}