using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Search;

public class SearchSpecialProjectQueryValidator : PaginationRequestValidator<SpecialProjectModel, SearchSpecialProjectQuery>;