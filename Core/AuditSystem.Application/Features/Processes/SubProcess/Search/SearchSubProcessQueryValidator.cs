using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Application.Features.Processes.SubProcess.Search;

public class SearchSubProcessQueryValidator : PaginationRequestValidator<SubProcessModel, SearchSubProcessQuery>;