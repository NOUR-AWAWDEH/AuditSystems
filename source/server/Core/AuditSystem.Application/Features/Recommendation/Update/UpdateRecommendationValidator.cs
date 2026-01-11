using FluentValidation;

namespace AuditSystem.Application.Features.Recommendation.Update;

public sealed class UpdateRecommendationValidator : AbstractValidator<UpdateRecommendationCommand>
{
    public UpdateRecommendationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

            
        RuleFor(x => x.ObservationId)
            .NotEmpty()
            .WithMessage("Observation Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Observation Id must not be empty.");
    }
    
    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}