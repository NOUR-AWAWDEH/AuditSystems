using FluentValidation;

namespace AuditSystem.Application.Features.Checklists.Checklist.Get;

public sealed class GetChecklistByIdQueryValidator : AbstractValidator<GetChecklistByIdQuery>
{
    public GetChecklistByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Checklist Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Checklist Id must be a valid Guid");  
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
