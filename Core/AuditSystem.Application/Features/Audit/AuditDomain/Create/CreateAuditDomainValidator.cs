using AuditSystem.Application.Features.Audit.AuditDomain.Create;
using FluentValidation;

public sealed class CreateAuditDomainValidator : AbstractValidator<CreateAuditDomainCommand>
{
    public CreateAuditDomainValidator()
    {
        RuleFor(x => x.DomainName)
            .NotEmpty()
            .WithMessage("The Domain name field is required.")
            .MinimumLength(3)
            .WithMessage("The Domain name must be at least 3 characters long.")
            .MaximumLength(255)
            .WithMessage("The Domain name must not exceed 255 characters.");
    }
}