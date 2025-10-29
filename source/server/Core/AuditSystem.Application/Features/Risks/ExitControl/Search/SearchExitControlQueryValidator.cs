using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Risks;
using FluentValidation;

namespace AuditSystem.Application.Features.Risks.ExitControl.Search;

public class SearchExitControlQueryValidator : PaginationRequestValidator<ExitControlModel, SearchExitControlQuery> 
{
    public SearchExitControlQueryValidator()
    { 
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");
        
        RuleFor(x => x.ObjectiveId)
            .NotEmpty()
            .WithMessage("Objective ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("Objective ID must be a valid GUID.");

    }
    
    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}