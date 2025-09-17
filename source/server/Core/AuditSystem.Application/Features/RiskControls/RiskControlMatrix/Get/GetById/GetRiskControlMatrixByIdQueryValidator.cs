using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Get.GetById;


public sealed class GetRiskControlMatrixByIdQueryValidator : AbstractValidator<GetRiskControlMatrixByIdQuery>
{
    public GetRiskControlMatrixByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Risk Control Matrix Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Risk Control Matrix Id must be a valid Guid");

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