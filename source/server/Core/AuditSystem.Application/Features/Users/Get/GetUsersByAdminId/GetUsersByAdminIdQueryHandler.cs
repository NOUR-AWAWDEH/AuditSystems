using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Users.Get.GetUsersByAdminId;

internal sealed class GetUsersByAdminIdQueryHandler(
    IAppUserService userService,
    ILogger<GetUsersByAdminIdQueryHandler> logger)
    : IRequestHandler<GetUsersByAdminIdQuery, Result<GetUsersByAdminIdQueryResponse>>
{
    public async Task<Result<GetUsersByAdminIdQueryResponse>> Handle(
        GetUsersByAdminIdQuery request,
        CancellationToken cancellationToken)
    {
        var users = await userService.GetUsersByAdminIdAsync(request.UserId);

        logger.LogInformation("{Count} users retrieved for admin {AdminId}",
            users.Count, request.UserId);

        return Result<GetUsersByAdminIdQueryResponse>.Success(
            new(users),
            "Users retrieved successfully.");
    }
}
