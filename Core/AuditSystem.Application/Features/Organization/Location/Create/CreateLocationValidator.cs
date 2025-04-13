using FluentValidation;

namespace AuditSystem.Application.Features.Organisation.LocationModle.Create;

public sealed class CreateLocationValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Location Name is required.")
            .MinimumLength(2)
            .WithMessage("Location Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Location Name must not exceed 200 characters.");

    }
}
