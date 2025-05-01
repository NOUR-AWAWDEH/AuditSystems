using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Search;

public class SearchBusinessObjectiveQueryValidator
    : PaginationRequestValidator<BusinessObjectiveModel, SearchBusinessObjectiveQuery>;