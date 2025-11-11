using FluentValidation;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Get.GetById;

public sealed class GetControlDesignByIdQueryValidator : AbstractValidator<GetControlDesignByIdQuery>
{
    public GetControlDesignByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Control Design Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Control Design Id must be a valid Guid");
        
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
