using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Skills;
using MediatR;

namespace AuditSystem.Application.Features.Skills.Skill.Search;

internal sealed class SearchSkillQueryHandler(ISkillService skillService) 
    : IRequestHandler<SearchSkillQuery, Result<PaginationRequestResponse<SkillModel>>>
{
    public async Task<Result<PaginationRequestResponse<SkillModel>>> Handle(SearchSkillQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await skillService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<SkillModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}