using FluentValidation;

namespace AuditSystem.Application.Features.Observation.Get.GetById;

public sealed class GetObservationByIdQueryValidator : AbstractValidator<GetObservationByIdQuery>
{
    public GetObservationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Observation Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Observation Id must be a valid Guid");
        
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
