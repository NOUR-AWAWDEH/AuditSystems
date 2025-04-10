using FluentValidation;

namespace AuditSystem.Application.Features.UserDesignation.Create;

public sealed class CreateUserDesignationValidator : AbstractValidator<CreateUserDesignationCommand>
{
    public CreateUserDesignationValidator()
    {
        RuleFor(x => x.Designation)
            .NotEmpty()
            .WithMessage("Designation is required.")
            .MinimumLength(2)
            .WithMessage("Designation must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Designation must not exceed 200 characters.");
        
       
        RuleFor(x => x.Level)
            .NotEmpty()
            .WithMessage("Level is required.")
            .MinimumLength(2)
            .WithMessage("Level must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Level must not exceed 200 characters.");


        RuleFor(x => x.IsActive)
            .NotNull()
            .WithMessage("IsActive is required.")
            .Must(x => x == true || x == false)
            .WithMessage("IsActive must be either true or false.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("UserId must be a valid GUID.");

        RuleFor(x => x.Description)
           .MaximumLength(500)
           .WithMessage("Description must not exceed 500 characters.");
    }
}
