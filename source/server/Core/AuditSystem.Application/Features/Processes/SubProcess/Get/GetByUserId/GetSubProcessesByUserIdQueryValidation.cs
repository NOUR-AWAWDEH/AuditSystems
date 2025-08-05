using FluentValidation;

namespace AuditSystem.Application.Features.Processes.SubProcess.Get.GetByUserId;

public sealed class GetSubProcessesByUserIdQueryValidator : AbstractValidator<GetSubProcessesByUserIdQuery>
{
    public GetSubProcessesByUserIdQueryValidator()
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