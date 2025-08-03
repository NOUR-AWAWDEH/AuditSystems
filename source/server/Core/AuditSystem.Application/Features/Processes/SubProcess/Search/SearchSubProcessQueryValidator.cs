using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Processes;
using FluentValidation;

namespace AuditSystem.Application.Features.Processes.SubProcess.Search;

public class SearchSubProcessQueryValidator : PaginationRequestValidator<SubProcessModel, SearchSubProcessQuery>
{
    public SearchSubProcessQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");
        
        RuleFor(x => x.AuditProcessId)
            .NotEmpty()
            .WithMessage("Audit Process ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("Audit Process ID must be a valid GUID");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .WithMessage("Search term must not exceed 200 characters.");
    }
    
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}