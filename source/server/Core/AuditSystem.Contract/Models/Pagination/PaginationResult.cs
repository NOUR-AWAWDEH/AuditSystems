
namespace AuditSystem.Contract.Models.Pagination;

public sealed class PaginationResult<TModel>
{
    public required IReadOnlyList<TModel> Items { get; init; }
    public required int TotalCount { get; init; }
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }

    public static PaginationResult<TModel> Empty => new()
    {
        Items = [],
        TotalCount = 0,
        PageNumber = 1,
        PageSize = 0
    };
}