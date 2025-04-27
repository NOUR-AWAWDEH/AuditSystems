using FluentValidation;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Get;

public sealed class GetBusinessObjectiveByIdQueryValidator : AbstractValidator<GetBusinessObjectiveByIdQuery>
{
    public GetBusinessObjectiveByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Business Objective Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Business Objective Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }    
}