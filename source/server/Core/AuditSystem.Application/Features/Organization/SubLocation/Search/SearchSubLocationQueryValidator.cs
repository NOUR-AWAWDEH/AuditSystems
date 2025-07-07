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
            
        RuleFor(x => x.LocationId)
            .Must(BeAValidGuidWhenProvided)
            .WithMessage("Location Id must be a valid Guid when provided");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
    
    private bool BeAValidGuidWhenProvided(Guid? id)
    {
        return !id.HasValue || id.Value != Guid.Empty;
    }
}