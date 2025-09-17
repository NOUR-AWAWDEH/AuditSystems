
namespace AuditSystem.Contract.Models.Pagination;

public sealed class PaginationResult<TModel>
{
    public required IReadOnlyList<TModel> Items { get; init; }
    public required int TotalCount { get; init; }
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
}