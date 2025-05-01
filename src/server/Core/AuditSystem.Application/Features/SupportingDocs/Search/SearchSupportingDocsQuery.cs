using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;

namespace AuditSystem.Application.Features.SupportingDocs.Search;

public sealed record class SearchSupportingDocsQuery : PaginationRequest<SupportingDocModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}