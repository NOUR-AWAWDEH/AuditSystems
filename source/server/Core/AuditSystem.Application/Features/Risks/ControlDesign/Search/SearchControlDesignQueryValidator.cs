using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Risks;
using FluentValidation;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Search;

public class SearchControlDesignQueryValidator : PaginationRequestValidator<ControlDesignModel, SearchControlDesignQuery> 
{
    public SearchControlDesignQueryValidator()
    { 
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");
        
        RuleFor(x => x.ExitControlId)
            .NotEmpty()
            .WithMessage("Exit Control ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("Exit Control ID must be a valid GUID.");

    }
    
    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}