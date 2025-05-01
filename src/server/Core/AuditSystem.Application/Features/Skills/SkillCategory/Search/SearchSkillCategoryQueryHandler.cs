using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Skills;
using MediatR;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Search;

internal sealed class SearchSkillCategoryQueryHandler(ISkillCategoryService skillCategoryService) 
    : IRequestHandler<SearchSkillCategoryQuery, Result<PaginationRequestResponse<SkillCategoryModel>>>
{
    public async Task<Result<PaginationRequestResponse<SkillCategoryModel>>> Handle(SearchSkillCategoryQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await skillCategoryService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<SkillCategoryModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}