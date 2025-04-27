using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Search;

public class SearchAuditDomainQueryValidator 
    : PaginationRequestValidator<AuditDomainModel, SearchAuditDomainQuery>;
