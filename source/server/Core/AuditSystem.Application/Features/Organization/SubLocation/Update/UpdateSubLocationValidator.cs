using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubLocation.Update;

public sealed class UpdateSubLocationValidator : AbstractValidator<UpdateSubLocationCommand>
{
    public UpdateSubLocationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("LocationId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("LocationId must not be empty.");

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address must not exceed 500 characters.");
    }
}
