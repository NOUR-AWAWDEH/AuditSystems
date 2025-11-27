using FluentValidation;

namespace AuditSystem.Application.Features.Users.Get.GetActionOwners;

public sealed class GetActionOwnersQueryValidator : AbstractValidator<GetActionOwnersQuery>
{
    public GetActionOwnersQueryValidator()
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
