using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Location.Search;

public class SearchLocationQueryValidator : AbstractValidator<SearchLocationQuery>
{
    public SearchLocationQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");
    }
    
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}