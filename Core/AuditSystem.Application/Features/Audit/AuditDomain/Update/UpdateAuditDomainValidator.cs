using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Update;

public sealed class UpdateAuditDomainValidator : AbstractValidator<UpdateAuditDomainCommand>
{
    public UpdateAuditDomainValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must not be empty GUID.");

        RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Name is required.")
        .MinimumLength(2)
        .WithMessage("Name must be at least 2 characters long.")
        .MaximumLength(100)
        .WithMessage("Name must not exceed 100 characters.");
    }
}