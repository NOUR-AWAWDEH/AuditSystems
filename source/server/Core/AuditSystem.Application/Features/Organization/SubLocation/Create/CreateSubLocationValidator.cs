using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubLocation.Create;

public sealed class CreateSubLocationValidator : AbstractValidator<CreateSubLocationCommand>
{
    public CreateSubLocationValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");


        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("Location Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Location Id must be a valid GUID.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("SubLocation Name is required.")
            .MinimumLength(2)
            .WithMessage("SubLocation Name must be at least 2 characters.")
            .MaximumLength(200)
            .WithMessage("SubLocation Name must not exceed 200 characters.");

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address must not exceed 500 characters.");
    }
}