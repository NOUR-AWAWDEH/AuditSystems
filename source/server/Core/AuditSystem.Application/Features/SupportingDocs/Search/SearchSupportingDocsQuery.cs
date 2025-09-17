using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Contract.Interfaces;

namespace AuditSystem.Application.Features.SupportingDocs.Search;

public sealed record SearchSupportingDocsQuery(
    string? SearchTerm
) : PaginationRequest<SupportingDocModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}