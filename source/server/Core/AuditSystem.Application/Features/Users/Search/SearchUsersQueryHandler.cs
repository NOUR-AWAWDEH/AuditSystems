using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Application.Features.Common.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Users.Search;

internal sealed class SearchUsersQueryHandler(
    IAppUserService userService,
    ILogger<SearchUsersQueryHandler> logger)
    : IRequestHandler<SearchUsersQuery, Result<PaginationRequestResponse<UserModel>>>
{
    public async Task<Result<PaginationRequestResponse<UserModel>>> Handle(
        SearchUsersQuery request,
        CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);

        var result = await userService.SearchByAdminIdAsync(
            request.UserId,
            request.SearchTerm,
            page);

        logger.LogInformation("Search returned {Count} users for admin {AdminId}",
            result.TotalCount, request.UserId);

        var response = new PaginationRequestResponse<UserModel>(result.Items, result.TotalCount);
        return Result<PaginationRequestResponse<UserModel>>.Success(
            response,
            "Users fetched successfully.");
    }
}
