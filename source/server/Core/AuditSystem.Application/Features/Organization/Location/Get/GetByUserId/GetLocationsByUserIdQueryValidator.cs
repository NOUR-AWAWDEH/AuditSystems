using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Location.Get.GetByUserId;

public sealed class GetLocationsByUserIdQueryValidator : AbstractValidator<GetLocationsByUserIdQuery>
{
    public GetLocationsByUserIdQueryValidator()
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