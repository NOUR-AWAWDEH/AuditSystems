using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Application.Features.Skills.SkillSet.Search;

public sealed record class SearchSkillSetQuery : PaginationRequest<SkillSetModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}