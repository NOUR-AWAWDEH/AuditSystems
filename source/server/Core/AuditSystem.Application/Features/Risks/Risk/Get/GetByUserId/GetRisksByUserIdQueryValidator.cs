using FluentValidation;

namespace AuditSystem.Application.Features.Risks.Risk.Get.GetByUserId;

public sealed class GetRisksByUserIdQueryValidator : AbstractValidator<GetRisksByUserIdQuery>
{
    public GetRisksByUserIdQueryValidator()
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
