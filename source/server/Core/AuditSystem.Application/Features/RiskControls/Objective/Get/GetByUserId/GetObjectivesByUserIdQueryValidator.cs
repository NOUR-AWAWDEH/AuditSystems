using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.Objective.Get.GetByUserId;

public sealed class GetObjectivesByUserIdQueryValidator : AbstractValidator<GetObjectivesByUserIdQuery>
{
    public GetObjectivesByUserIdQueryValidator()
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