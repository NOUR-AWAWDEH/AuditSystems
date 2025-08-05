using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Application.Features.Common.Rating.Search;

public sealed record class SearchRatingQuery : PaginationRequest<RateModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}