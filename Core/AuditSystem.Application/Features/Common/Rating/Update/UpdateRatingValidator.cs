using FluentValidation;

namespace AuditSystem.Application.Features.Common.Rating.Update;

public sealed class UpdateRatingValidator : AbstractValidator<UpdateRatingCommand>
{
    public UpdateRatingValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must be a valid GUID.");

        RuleFor(x => x.Level)
            .NotEmpty()
            .WithMessage("Level is required.")
            .MaximumLength(50)
            .WithMessage("Level must not exceed 50 characters.");
    }
}

