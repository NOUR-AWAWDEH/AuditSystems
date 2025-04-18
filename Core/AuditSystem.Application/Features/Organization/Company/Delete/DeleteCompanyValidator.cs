using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Company.Delete;

public sealed class DeleteCompanyValidator : AbstractValidator<DeleteCompanyCommand>
{
    public DeleteCompanyValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Company ID is required.")
            .Must(IsValidGuid)
            .WithMessage("Company ID must be a valid GUID.");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}