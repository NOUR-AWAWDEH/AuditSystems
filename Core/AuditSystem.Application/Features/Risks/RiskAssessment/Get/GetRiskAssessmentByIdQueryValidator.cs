using FluentValidation;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Get;

public sealed class GetRiskAssessmentByIdQueryValidator : AbstractValidator<GetRiskAssessmentByIdQuery>
{
    public GetRiskAssessmentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Risk Assessment Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Risk Assessment Id must be a valid Guid");  
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
