using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Checklists;

namespace AuditSystem.Application.Features.Checklists.Checklist.Search;

public class SearchChecklistQueryValidator
    : PaginationRequestValidator<ChecklistModel, SearchChecklistQuery>;