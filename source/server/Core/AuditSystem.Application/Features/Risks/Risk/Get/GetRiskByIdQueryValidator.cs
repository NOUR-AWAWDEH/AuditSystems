using FluentValidation;

namespace AuditSystem.Application.Features.Risks.Risk.Get;

public sealed class GetRiskByIdQueryValidator : AbstractValidator<GetRiskByIdQuery>
{
    public GetRiskByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Risk Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Risk Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
