using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Search;

public class SearchAuditEngagementQueryValidator
    : PaginationRequestValidator<AuditEngagementModel, SearchAuditEngagementQuery>;