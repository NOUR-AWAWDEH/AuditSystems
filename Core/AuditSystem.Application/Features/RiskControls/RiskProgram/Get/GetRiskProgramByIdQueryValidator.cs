using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Get;

public sealed class GetRiskProgramByIdQueryValidator : AbstractValidator<GetRiskProgramByIdQuery>
{
    public GetRiskProgramByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Risk Program Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Risk Program Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;  
    }
}
