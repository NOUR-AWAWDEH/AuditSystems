using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Search;

public class SearchAuditProcessQueryValidator : PaginationRequestValidator<AuditProcessModel, SearchAuditProcessQuery>;