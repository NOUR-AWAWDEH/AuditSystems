using FluentValidation;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Update;

public sealed class UpdateAuditProcessValidator : AbstractValidator<UpdateAuditProcessCommand>
{
    public UpdateAuditProcessValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Id must be a valid Guid");

        RuleFor(x => x.ProcessName)
            .NotEmpty()
            .WithMessage("ProcessName is required.")
            .MinimumLength(2)
            .WithMessage("ProcessName must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("ProcessName must not exceed 100 characters.");

        RuleFor(x => x.SubDepartmentId)
            .NotEmpty()
            .WithMessage("SubDepartmentId is required")
            .Must(BeAValidGuid)
            .WithMessage("SubDepartmentId must be a valid Guid");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required")
            .Must(BeAValidGuid)
            .WithMessage("User Id must be a valid Guid");
    }

    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}