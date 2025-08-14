using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.Objective.Get.GetById;

public sealed class GetObjectiveByIdQueryValidator : AbstractValidator<GetObjectiveByIdQuery>
{
    public GetObjectiveByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Objective Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Objective Id must be a valid Guid");
        
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
