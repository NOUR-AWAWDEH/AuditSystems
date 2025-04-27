using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Get;

public sealed class GetRiskControlMatrixByIdQueryValidator : AbstractValidator<GetRiskControlMatrixByIdQuery>
{
    public GetRiskControlMatrixByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Risk Control Matrix Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Risk Control Matrix Id must be a valid Guid");  
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}