using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.Objective.Search;

public class SearchObjectiveQueryValidator
    : PaginationRequestValidator<ObjectiveModel, SearchObjectiveQuery>;