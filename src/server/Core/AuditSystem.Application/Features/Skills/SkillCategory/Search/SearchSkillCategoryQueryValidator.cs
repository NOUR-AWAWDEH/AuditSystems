using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Search;

public class SearchSkillCategoryQueryValidator 
    : PaginationRequestValidator<SkillCategoryModel, SearchSkillCategoryQuery>;