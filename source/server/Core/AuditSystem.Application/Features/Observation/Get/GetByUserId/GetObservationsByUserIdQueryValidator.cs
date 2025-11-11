using FluentValidation;

namespace AuditSystem.Application.Features.Observation.Get.GetByUserId;

public sealed class GetObservationsByUserIdQueryValidator : AbstractValidator<GetObservationsByUserIdQuery>
{
    public GetObservationsByUserIdQueryValidator()
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