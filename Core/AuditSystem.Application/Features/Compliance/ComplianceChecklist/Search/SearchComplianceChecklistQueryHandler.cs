using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Search;

internal sealed class SearchComplianceChecklistQueryHandler(IComplianceChecklistService complianceChecklistService)
    : IRequestHandler<SearchComplianceChecklistQuery, Result<PaginationRequestResponse<ComplianceChecklistModel>>>
{
    public async Task<Result<PaginationRequestResponse<ComplianceChecklistModel>>> Handle(SearchComplianceChecklistQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await complianceChecklistService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<ComplianceChecklistModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}