using FluentValidation;

namespace AuditSystem.Application.Features.Users.Get.GetUsersByAdminId;

public sealed class GetUsersByAdminIdQueryValidator : AbstractValidator<GetUsersByAdminIdQuery>
{
    public GetUsersByAdminIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(BeAValidGuid).WithMessage("Invalid user ID.");
    }
    
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
