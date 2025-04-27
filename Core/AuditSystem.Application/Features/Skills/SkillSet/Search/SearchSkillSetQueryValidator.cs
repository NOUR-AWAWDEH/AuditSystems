using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Application.Features.Skills.SkillSet.Search;

public class SearchSkillSetQueryValidator : PaginationRequestValidator<SkillSetModel, SearchSkillSetQuery>;