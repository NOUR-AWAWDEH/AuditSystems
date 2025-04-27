using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Compliance;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Search;

public class SearchComplianceChecklistQueryValidator : PaginationRequestValidator<ComplianceChecklistModel, SearchComplianceChecklistQuery>;