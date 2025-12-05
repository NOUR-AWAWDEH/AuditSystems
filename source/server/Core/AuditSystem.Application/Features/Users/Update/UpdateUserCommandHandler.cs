using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using AuditSystem.Contract.Models.Users;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Users.Update;

internal sealed class UpdateUserCommandHandler(
    IAppUserService userService,
    IMapper mapper,
    ILogger<UpdateUserCommandHandler> logger)
    : IRequestHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var userModel = mapper.Map<UserModel>(request);

        await userService.UpdateUserAsync(userModel, request.UserId);

        logger.LogInformation("User {UserId} updated successfully by admin {AdminId}",
            request.Id, request.UserId);

        return Result.SuccessWithMessage("User updated successfully.");
    }
}
