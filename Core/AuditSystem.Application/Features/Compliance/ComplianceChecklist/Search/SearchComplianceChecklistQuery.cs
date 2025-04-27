using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Compliance;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Search;

public sealed record class SearchComplianceChecklistQuery : PaginationRequest<ComplianceChecklistModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}