using FluentValidation;

namespace AuditSystem.Application.Features.Common.Rating.Get;

public sealed class GetRateByIdQueryValidator : AbstractValidator<GetRateByIdQuery>
{
    public GetRateByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Rate Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Rate Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}