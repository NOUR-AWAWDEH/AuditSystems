using FluentValidation;

namespace AuditSystem.Application.Features.Common.Rating.Get;

public sealed class GetRatingByIdQueryValidator : AbstractValidator<GetRatingByIdQuery>
{
    public GetRatingByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Rating Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Rating Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}