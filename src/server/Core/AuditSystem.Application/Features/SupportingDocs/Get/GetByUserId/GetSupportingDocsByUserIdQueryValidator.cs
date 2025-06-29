using FluentValidation;

namespace AuditSystem.Application.Features.SupportingDocs.Get.GetByUserId;

public sealed class GetSupportingDocsByUserIdQueryValidator : AbstractValidator<GetSupportingDocsByUserIdQuery>
{
    public GetSupportingDocsByUserIdQueryValidator()
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