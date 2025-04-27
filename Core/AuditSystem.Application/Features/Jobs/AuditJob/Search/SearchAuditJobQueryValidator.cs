using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Search;

public class SearchAuditJobQueryValidator : PaginationRequestValidator<AuditJobModel, SearchAuditJobQuery>;