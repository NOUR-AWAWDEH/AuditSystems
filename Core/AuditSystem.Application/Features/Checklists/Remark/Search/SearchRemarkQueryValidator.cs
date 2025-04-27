using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Checklists;

namespace AuditSystem.Application.Features.Checklists.Remark.Search;

public class SearchRemarkQueryValidator : PaginationRequestValidator<RemarkModel, SearchRemarkQuery>;