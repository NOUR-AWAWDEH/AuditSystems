using Ardalis.Result;
using AuditSystem.Application.Constants;
using MediatR;
namespace AuditSystem.Application.Features.Common.Pagination;

public abstract record PaginationRequest<TModel> : IRequest<Result<PaginationRequestResponse<TModel>>>
{
    public int PageNumber { get; set; } = PaginationConstants.DefaultPageNumber;
    public int PageSize { get; set; } = PaginationConstants.DefaultPageSize;
}
