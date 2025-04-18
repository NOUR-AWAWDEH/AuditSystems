using FluentValidation;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Delete;

public sealed class DeleteBusinessObjectiveValidator : AbstractValidator<DeleteBusinessObjectiveCommand>
{
    public DeleteBusinessObjectiveValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Business Objective Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Business Objective Id is not a valid GUID.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}
