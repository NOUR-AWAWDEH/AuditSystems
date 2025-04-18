using AuditSystem.Application.Common;
using FluentValidation;

namespace AuditSystem.Application.Features.Common.Rating.Create;

public sealed class CreateRatingValidator : AbstractValidator<CreateRatingCommand>
{
    public CreateRatingValidator()
    {
        RuleFor(x => x.Level)
            .NotEmpty()
            .Must(RatingValidator.IsValidLevel)
            .WithMessage("Invalid rating level. Valid values are: " + string.Join(", ", RatingValidator.ValidLevels));
    }
}