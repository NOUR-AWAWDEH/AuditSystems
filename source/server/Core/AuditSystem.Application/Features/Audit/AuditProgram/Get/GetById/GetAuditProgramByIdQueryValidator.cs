using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Get.GetById;

public sealed class GetAuditProgramByIdQueryValidator : AbstractValidator<GetAuditProgramByIdQuery>
{
    public GetAuditProgramByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Program Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Audit Program Id must be a valid Guid");
        
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
