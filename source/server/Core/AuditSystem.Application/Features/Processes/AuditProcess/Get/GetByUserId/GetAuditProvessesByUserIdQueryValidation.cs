using FluentValidation;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Get.GetByUserId;

public sealed class GetAuditProcessesByUserIdQueryValidator : AbstractValidator<GetAuditProcessesByUserIdQuery>
{
    public GetAuditProcessesByUserIdQueryValidator()
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