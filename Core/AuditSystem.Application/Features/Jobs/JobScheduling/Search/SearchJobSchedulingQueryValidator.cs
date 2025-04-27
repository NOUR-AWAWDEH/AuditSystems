using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Search;

public class SearchJobSchedulingQueryValidator
    : PaginationRequestValidator<JobSchedulingModel, SearchJobSchedulingQuery>;