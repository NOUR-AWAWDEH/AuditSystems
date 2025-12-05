using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Users.Delete;

internal sealed class DeleteUserCommandHandler(
    IAppUserService userService,
    ILogger<DeleteUserCommandHandler> logger)
    : IRequestHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        await userService.DeleteUserAsync(request.Id, request.UserId);

        logger.LogInformation("User {UserId} deleted successfully by admin {AdminId}",
            request.Id, request.UserId);

        return Result.SuccessWithMessage("User deleted successfully.");
    }
}
