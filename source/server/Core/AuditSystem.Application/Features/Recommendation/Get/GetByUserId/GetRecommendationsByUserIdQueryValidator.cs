using FluentValidation;

namespace AuditSystem.Application.Features.Recommendation.Get.GetByUserId;

public sealed class GetRecommendationsByUserIdQueryValidator : AbstractValidator<GetRecommendationsByUserIdQuery>
{
    public GetRecommendationsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required")
            .Must(BeAValidGuid)
            .WithMessage("User Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}