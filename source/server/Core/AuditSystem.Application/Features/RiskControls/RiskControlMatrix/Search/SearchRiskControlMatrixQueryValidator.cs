using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Search;

public class SearchRiskControlMatrixQueryValidator : PaginationRequestValidator<RiskControlMatrixModel, SearchRiskControlMatrixQuery>
{
    public SearchRiskControlMatrixQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");
        
        RuleFor(x => x.SubProcessId)
            .NotEmpty()
            .WithMessage("SubProcess ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("SubProcess ID must be a valid GUID");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .WithMessage("Search term must not exceed 200 characters.");
    }
    
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}