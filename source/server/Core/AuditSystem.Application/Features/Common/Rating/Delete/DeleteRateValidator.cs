using FluentValidation;

namespace AuditSystem.Application.Features.Common.Rating.Delete;

public sealed class DeleteRateValidator : AbstractValidator<DeleteRateCommand>
{
    public DeleteRateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Rate Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Rate Id cannot be empty.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}