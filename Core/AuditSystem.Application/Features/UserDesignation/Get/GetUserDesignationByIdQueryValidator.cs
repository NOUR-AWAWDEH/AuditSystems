using FluentValidation;

namespace AuditSystem.Application.Features.UserDesignation.Get;

public sealed class GetUserDesignationByIdQueryValidator : AbstractValidator<GetUserDesignationByIdQuery>
{
    public GetUserDesignationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User Designation Id is required")
            .Must(BeAValidGuid)
            .WithMessage("User Designation Id must be a valid Guid");  
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
