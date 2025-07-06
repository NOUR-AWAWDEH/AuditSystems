using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Create;
public sealed class CreateAuditDomainValidator : AbstractValidator<CreateAuditDomainCommand>
{
    public CreateAuditDomainValidator()
    {
        RuleFor(x => x.Name)
            .NotNull() // Ensures `null` is rejected
            .WithMessage("The Domain name cannot be null.")
            .NotEmpty() // Ensures `""` (empty string) is rejected
            .WithMessage("The Domain name field is required.")
            .MinimumLength(2) // Ensures at least 2 characters
            .WithMessage("The Domain name must be at least 2 characters long.")
            .MaximumLength(255) // Ensures no more than 255 characters
            .WithMessage("The Domain name must not exceed 255 characters.");
    }
}