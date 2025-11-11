using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Company.Create;
public sealed class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Company Name is required.")
            .MinimumLength(2)
            .WithMessage("Company Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Company Name must not exceed 200 characters.");
    }
}