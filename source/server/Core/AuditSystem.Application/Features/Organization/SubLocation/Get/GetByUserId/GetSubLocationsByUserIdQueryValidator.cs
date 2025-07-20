using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get.GetByUserId;

public sealed class GetSubLocationsByUserIdQueryValidator : AbstractValidator<GetSubLocationsByUserIdQuery>
{
    public GetSubLocationsByUserIdQueryValidator()
    {

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
