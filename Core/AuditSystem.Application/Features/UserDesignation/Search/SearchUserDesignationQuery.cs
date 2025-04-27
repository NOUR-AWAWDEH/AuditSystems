using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.UserDesignation;

namespace AuditSystem.Application.Features.UserDesignation.Search;

public sealed record class SearchUserDesignationQuery : PaginationRequest<UserDesignationModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}