using FluentValidation;

namespace AuditSystem.Application.Features.SupportingDocs.Get.GetURLByFileId;

public sealed class GetSupportingDocURLQueryByIdValidator : AbstractValidator<GetSupportingDocURLQueryById>
{
    public GetSupportingDocURLQueryByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Supporting Doc Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Supporting Doc Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
