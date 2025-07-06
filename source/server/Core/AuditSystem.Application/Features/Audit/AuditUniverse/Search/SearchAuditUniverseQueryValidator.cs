using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Search;

public class SearchAuditUniverseQueryValidator 
    : PaginationRequestValidator<AuditUniverseModel, SearchAuditUniverseQuery>;