using FluentValidation;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Create;

public sealed class CreateAuditProcessValidator : AbstractValidator<CreateAuditProcessCommand>
{
    public CreateAuditProcessValidator()
    {

        RuleFor(x => x.ProcessName)
            .NotEmpty()
            .WithMessage("Audit Process Name is required.")
            .MinimumLength(2)
            .WithMessage("Audit Process Name must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Audit Process Name must not exceed 500 characters.");

       RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");

        RuleFor(x => x.SubDepartmentId)
            .NotEmpty()
            .WithMessage("Sub Department Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Sub Department Id must be a valid GUID.");
    }
}