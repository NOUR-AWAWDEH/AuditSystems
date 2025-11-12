using FluentValidation;

namespace AuditSystem.Application.Features.Recommendation.Get.GetById;

public sealed class GetRecommendationByIdQueryValidator : AbstractValidator<GetRecommendationByIdQuery>
{
    public GetRecommendationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Recommendation Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Recommendation Id must be a valid Guid");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("User Id must be a valid GUID.");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
