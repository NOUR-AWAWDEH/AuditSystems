using FluentValidation;

namespace AuditSystem.Application.Features.Recommendation.Create;

public sealed class CreateRecommendationCommandValidator : AbstractValidator<CreateRecommendationCommand>
{
    public CreateRecommendationCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Recommendation Description is required.")
            .MinimumLength(2)
            .WithMessage("Recommendation Description must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Recommendation Description must not exceed 500 characters.");

        
        RuleFor(x => x.ObservationId)
            .NotEmpty()
            .WithMessage("Observation Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Observation Id must be a valid GUID.");
    }
}
