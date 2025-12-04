using FluentValidation;

namespace AuditSystem.Application.Features.Organization.JobTitle.Create;

public class CreateJobTitleValidator : AbstractValidator<CreateJobTitleCommand>
{
    public CreateJobTitleValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("Company Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Company Id must be a valid GUID.");
    }
}