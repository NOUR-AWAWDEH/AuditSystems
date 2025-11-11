using FluentValidation;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Application.Features.Common.Rating.Update;

public sealed class UpdateRateValidator : AbstractValidator<UpdateRateCommand>
{
    public UpdateRateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must be a valid GUID.");

        RuleFor(x => x.Level)
            .IsInEnum()
            .WithMessage("Invalid rate level. Valid values are: " + string.Join(", ", Enum.GetNames<RateLevel>()));
    }
}
