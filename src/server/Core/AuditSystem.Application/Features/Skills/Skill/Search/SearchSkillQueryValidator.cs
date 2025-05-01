using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Application.Features.Skills.Skill.Search;

public class SearchSkillQueryValidator 
    : PaginationRequestValidator<SkillModel, SearchSkillQuery>;