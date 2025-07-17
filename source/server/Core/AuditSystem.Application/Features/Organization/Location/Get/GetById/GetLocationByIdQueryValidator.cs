using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Location.Get.GetById;

public sealed class GetLocationByIdQueryValidator : AbstractValidator<GetLocationByIdQuery>
{
    public GetLocationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Location Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Location Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}