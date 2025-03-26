using FluentValidation;

namespace AuditSystem.Application.Features.Organisation.SubLocation.Create;

public sealed class CreateSubLocationValidator : AbstractValidator<CreateSubLocationCommand>
{
    public CreateSubLocationValidator() 
    {
        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("Location Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("SubLocation Name is required.")
            .MaximumLength(200)
            .WithMessage("SubLocation Name must not exceed 200 characters.");
    }
}
