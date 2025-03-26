using FluentValidation;

namespace AuditSystem.Application.Features.Organisation.Location.Create;

public sealed class CreateLocationValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationValidator() 
    {
        RuleFor(x => x.AuditorSettingsId)
            .NotEmpty()
            .WithMessage("Auditor Settings Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Location Name is required.")
            .MaximumLength(200)
            .WithMessage("Location Name must not exceed 200 characters.");

    }
}
