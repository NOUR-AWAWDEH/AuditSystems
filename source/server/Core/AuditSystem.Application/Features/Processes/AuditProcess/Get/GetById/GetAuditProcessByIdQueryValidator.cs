using FluentValidation;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Get.GetById;

public sealed class GetAuditProcessByIdQueryValidator : AbstractValidator<GetAuditProcessByIdQuery>
{
    public GetAuditProcessByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Process Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Audit Process Id must be a valid Guid");
        
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