using FluentValidation;

namespace AuditSystem.Application.Features.Checklists.Remark.Get;

public sealed class GetRemarkByIdQueryValidator : AbstractValidator<GetRemarkByIdQuery>
{
    public GetRemarkByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Remark Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Remark Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }    
}