using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Get;

public sealed class GetRiskControlsByIdQueryValidator : AbstractValidator<GetRiskControlsByIdQuery>
{
    public GetRiskControlsByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Risk Control Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Risk Control Id must be a valid Guid");  
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
