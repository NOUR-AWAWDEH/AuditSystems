using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Tasks;

namespace AuditSystem.Application.Features.Tasks.Search;

public class SearchTaskManagementQueryValidator 
    : PaginationRequestValidator<TaskManagementModel, SearchTaskManagementQuery>;