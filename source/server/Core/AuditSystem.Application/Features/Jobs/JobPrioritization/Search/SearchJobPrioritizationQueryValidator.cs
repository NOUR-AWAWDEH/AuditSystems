using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Search;

public class SearchJobPrioritizationQueryValidator
    : PaginationRequestValidator<JobPrioritizationModel, SearchJobPrioritizationQuery>;