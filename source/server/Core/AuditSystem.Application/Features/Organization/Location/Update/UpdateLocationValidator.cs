using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Location.Update;

public sealed class UpdateLocationValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");
        
        RuleFor(x => x.Address)
            .MaximumLength(100)
            .WithMessage("Address must not exceed 100 characters.");
        
        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("CompanyId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("CompanyId must not be empty.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("UserId must not be empty.");
    }
}

