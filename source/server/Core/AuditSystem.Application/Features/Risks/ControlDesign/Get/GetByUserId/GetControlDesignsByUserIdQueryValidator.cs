using FluentValidation;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Get.GetByUserId;

public sealed class GetControlDesignsByUserIdQueryValidator : AbstractValidator<GetControlDesignsByUserIdQuery>
{
    public GetControlDesignsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required")
            .Must(BeAValidGuid)
            .WithMessage("User Id must be a valid Guid");
    }

    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
