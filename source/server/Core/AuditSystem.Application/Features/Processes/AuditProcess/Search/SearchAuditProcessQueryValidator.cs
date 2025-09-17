using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Processes;
using FluentValidation;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Search;

public class SearchAuditProcessQueryValidator : PaginationRequestValidator<AuditProcessModel, SearchAuditProcessQuery>
{
    public SearchAuditProcessQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");
        
        RuleFor(x => x.SubDepartmentId)
            .Must(BeAValidGuid)
            .WithMessage("SubDepartment ID must be a valid GUID");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .WithMessage("Search term must not exceed 200 characters.");
    }
    
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}