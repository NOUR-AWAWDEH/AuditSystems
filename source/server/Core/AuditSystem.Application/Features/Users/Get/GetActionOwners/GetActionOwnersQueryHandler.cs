using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Users.Get.GetActionOwners;

internal sealed class GetActionOwnersQueryHandler(
    IAppUserService userService,
    ILogger<GetActionOwnersQueryHandler> logger) :
    IRequestHandler<GetActionOwnersQuery, Result<GetActionOwnersQueryResponse>>
{
    public async Task<Result<GetActionOwnersQueryResponse>> Handle(
        GetActionOwnersQuery request,
        CancellationToken cancellationToken)
    {
        var actionOwners = await userService.GetActionOwnersByAdminIdAsync(request.UserId);
        logger.LogInformation("Action owners retrieved successfully");
        return Result<GetActionOwnersQueryResponse>.Success(new(actionOwners), "Action owners retrieved successfully");
    }
}
