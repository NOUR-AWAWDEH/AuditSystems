using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Get.GetByUserId;

public sealed class GetAuditProgramsByUserIdQueryValidator : AbstractValidator<GetAuditProgramsByUserIdQuery>
{
    public GetAuditProgramsByUserIdQueryValidator()
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