using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Search;

public class SearchAuditUniverseObjectiveQueryValidator
    : PaginationRequestValidator<AuditUniverseObjectiveModel, SearchAuditUniverseObjectiveQuery>;