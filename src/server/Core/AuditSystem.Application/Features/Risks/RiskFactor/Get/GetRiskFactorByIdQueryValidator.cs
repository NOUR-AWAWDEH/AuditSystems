using FluentValidation;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Get;

public sealed class GetRiskFactorByIdQueryValidator : AbstractValidator<GetRiskFactorByIdQuery>
{
    public GetRiskFactorByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Risk Factor Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Risk Factor Id must be a valid Guid");  
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
