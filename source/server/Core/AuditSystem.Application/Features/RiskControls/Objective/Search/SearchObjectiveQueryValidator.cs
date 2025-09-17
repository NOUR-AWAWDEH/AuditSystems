using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.Objective.Search;

public class SearchObjectiveQueryValidator : PaginationRequestValidator<ObjectiveModel, SearchObjectiveQuery>
{
    public SearchObjectiveQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");
        
        RuleFor(x => x.RiskControlMatrixId)
            .NotEmpty()
            .WithMessage("Risk control matrix ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("Risk control matrix ID must be a valid GUID");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .WithMessage("Search term must not exceed 200 characters.");
    }
    
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
