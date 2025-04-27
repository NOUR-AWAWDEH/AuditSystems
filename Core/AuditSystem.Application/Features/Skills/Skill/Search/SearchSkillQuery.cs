using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Application.Features.Skills.Skill.Search;

public sealed record class SearchSkillQuery : PaginationRequest<SkillModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}