using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubLocation.Search;

public sealed class SearchSubLocationQueryValidator : AbstractValidator<SearchSubLocationQuery>
{
    public SearchSubLocationQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required")
            .Must(BeAValidGuid)
            .WithMessage("User Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}