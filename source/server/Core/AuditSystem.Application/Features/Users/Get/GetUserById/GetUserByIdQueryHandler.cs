using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Users.Get.GetUserById;

internal sealed class GetUserByIdQueryHandler(
    IAppUserService userService,
    ILogger<GetUserByIdQueryHandler> logger)
    : IRequestHandler<GetUserByIdQuery, Result<GetUserByIdQueryResponse>>
{
    public async Task<Result<GetUserByIdQueryResponse>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(request.Id, request.UserId);

        logger.LogInformation("User {UserId} retrieved successfully by admin {AdminId}",
            request.Id, request.UserId);

        return Result<GetUserByIdQueryResponse>.Success(
            new(user),
            "User retrieved successfully.");
    }
}
