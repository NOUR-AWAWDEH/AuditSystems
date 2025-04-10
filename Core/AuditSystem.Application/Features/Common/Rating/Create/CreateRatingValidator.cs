using AuditSystem.Application.Features.Common.Rating.Create;
using AuditSystem.Domain.Entities.Common;
using FluentValidation;

public sealed class CreateRatingValidator : AbstractValidator<CreateRatingCommand>
{
    public CreateRatingValidator()
    {
        RuleFor(x => x.Level)
            .NotEmpty()
            .Must(level => Rating.ValidLevels.Contains(level))
            .WithMessage("Invalid rating level. Valid values are: " + string.Join(", ", Rating.ValidLevels));
    }
}