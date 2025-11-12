using FluentValidation;

namespace AuditSystem.Application.Features.Recommendation.Delete;

public sealed class DeleteRecommendationValidator : AbstractValidator<DeleteRecommendationCommand>
{
    public DeleteRecommendationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Recommendation ID is required.")
            .Must(IsValidGuid)
            .WithMessage("Recommendation ID must be a valid GUID.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(IsValidGuid)
            .WithMessage("User Id must be a valid GUID.");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
