using FluentValidation;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Get;

public sealed class GetSpecialProjectByIdQueryValidator : AbstractValidator<GetSpecialProjectByIdQuery>
{
    public GetSpecialProjectByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Special Project Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Special Project Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
