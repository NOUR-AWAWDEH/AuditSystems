using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Skills;
using MediatR;

namespace AuditSystem.Application.Features.Skills.SkillSet.Search;

internal sealed class SearchSkillSetQueryHandler(ISkillSetService skillSetService) 
    : IRequestHandler<SearchSkillSetQuery, Result<PaginationRequestResponse<SkillSetModel>>>
{
    public async Task<Result<PaginationRequestResponse<SkillSetModel>>> Handle(SearchSkillSetQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await skillSetService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<SkillSetModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}