using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using MediatR;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Search;

internal sealed class SearchRiskAssessmentQueryHandler(IRiskAssessmentService riskAssessmentService) :
    IRequestHandler<SearchRiskAssessmentQuery, Result<PaginationRequestResponse<RiskAssessmentModel>>>
{
    public async Task<Result<PaginationRequestResponse<RiskAssessmentModel>>> Handle(SearchRiskAssessmentQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await riskAssessmentService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<RiskAssessmentModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}