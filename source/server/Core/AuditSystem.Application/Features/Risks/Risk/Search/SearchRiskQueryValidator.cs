using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Risks;
using FluentValidation;

namespace AuditSystem.Application.Features.Risks.Risk.Search;

public class SearchRiskQueryValidator : PaginationRequestValidator<RiskModel, SearchRiskQuery> 
{
    public SearchRiskQueryValidator()
    { 
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");
        
        RuleFor(x => x.ObservationId)
            .NotEmpty()
            .WithMessage("Observation ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("Observation ID must be a valid GUID.");

    }
    
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}