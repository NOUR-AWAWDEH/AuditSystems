using AuditSystem.Application.Features.Organisation.Company.Create;
using FluentValidation;

public sealed class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Company Name is required.")
            .MaximumLength(200)
            .WithMessage("Company Name must not exceed 200 characters.");
    }
}