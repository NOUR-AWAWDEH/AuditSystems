using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Location.Create;

public sealed class CreateLocationValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Location Name is required.")
            .MinimumLength(2)
            .WithMessage("Location Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Location Name must not exceed 200 characters.");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("Company Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Company Id must be a valid GUID.");
        
        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address must not exceed 500 characters.");
    }
}
