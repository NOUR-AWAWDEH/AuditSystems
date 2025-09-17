using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get.GetById;

public sealed class GetSubLocationByIdQueryValidator : AbstractValidator<GetSubLocationByIdQuery>
{
    public GetSubLocationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sub Location Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Sub Location Id must be a valid Guid");

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
