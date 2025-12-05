using FluentValidation;

namespace AuditSystem.Application.Features.Users.Get.GetUserById;

public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(BeAValidGuid).WithMessage("Invalid user ID.");
    }

    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
