using FluentValidation;

namespace AuditSystem.Application.Features.UserDesignation.Update;

public sealed class UpdateUserDesignationValidator : AbstractValidator<UpdateUserDesignationCommand>
{
    public UpdateUserDesignationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.Designation)
            .NotEmpty()
            .WithMessage("Designation is required.")
            .MinimumLength(2)
            .WithMessage("Designation must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Designation must not exceed 100 characters.");

        RuleFor(x => x.Level)
            .NotEmpty()
            .WithMessage("Level is required.")
            .MinimumLength(2)
            .WithMessage("Level must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("Level must not exceed 50 characters.");

        RuleFor(x => x.IsActive)
            .NotNull()
            .WithMessage("IsActive is required.")
            .Must(isActive => isActive == true || isActive == false)
            .WithMessage("IsActive must be a boolean value (true or false).");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("UserId must not be empty.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}
