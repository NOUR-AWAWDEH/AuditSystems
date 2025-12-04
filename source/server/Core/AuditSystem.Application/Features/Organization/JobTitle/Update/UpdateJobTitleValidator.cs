using FluentValidation;

namespace AuditSystem.Application.Features.Organization.JobTitle.Update;

public class UpdateJobTitleValidator : AbstractValidator<UpdateJobTitleCommand>
{
    public UpdateJobTitleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Job Title ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("Job Title ID must be a valid GUID.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Job Title Name is required.")
            .MaximumLength(200)
            .WithMessage("Job Title Name must not exceed 200 characters.");

        RuleFor(x => x.CompanyId)
            .Must(BeAValidGuid)
            .WithMessage("CompanyId ID must be a valid GUID");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}