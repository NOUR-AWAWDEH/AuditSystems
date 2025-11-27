using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Users.Get.GetActionOwners;

internal sealed class GetActionOwnersQueryHandler(
    IUserService userService,
    ILogger<GetActionOwnersQueryHandler> logger) : 
    IRequestHandler<GetActionOwnersQuery, Result<GetActionOwnersQueryResponse>>
{
    public async Task<Result<GetActionOwnersQueryResponse>> Handle(
        GetActionOwnersQuery request, 
        CancellationToken cancellationToken)
    {
        var actionOwners = await userService.GetActionOwnersByUserIdAsync(request.UserId);

        return Result<GetActionOwnersQueryResponse>.Success(new(actionOwners));
    }
}
