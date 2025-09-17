using FluentValidation;

namespace AuditSystem.Application.Features.Risks.ExitControl.Get.GetById;

public sealed class GetExitControlByIdQueryValidator : AbstractValidator<GetExitControlByIdQuery>
{
    public GetExitControlByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Exit Control Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Exit Control Id must be a valid Guid");
        
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
