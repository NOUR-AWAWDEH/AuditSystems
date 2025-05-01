using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Search;

public sealed record class SearchSkillCategoryQuery : PaginationRequest<SkillCategoryModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}