using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubLocation.Search;

public sealed class SearchSubLocationQueryValidator : AbstractValidator<SearchSubLocationQuery>
{
    public SearchSubLocationQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");
            
        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("Location ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("Location ID must be a valid GUID.");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}