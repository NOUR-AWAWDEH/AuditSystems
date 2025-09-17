using AuditSystem.Application.Common.Validators;
using AuditSystem.Domain.Entities.Common;
using FluentValidation;

namespace AuditSystem.Application.Features.Common.Rating.Create;

public sealed class CreateRateValidator : AbstractValidator<CreateRateCommand>
{
    public CreateRateValidator()
    {
        RuleFor(x => x.Level)
            .IsInEnum()
            .WithMessage("Invalid rate level. Valid values are: " + string.Join(", ", Enum.GetNames<RateLevel>()));
    }
}