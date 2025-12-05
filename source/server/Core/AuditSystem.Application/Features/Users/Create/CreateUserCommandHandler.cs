using Ardalis.Result;
using AuditSystem.Contract.Dtos.Register;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Users.Create;

internal sealed class CreateUserCommandHandler(
    IAppUserService userService,
    IMapper mapper,
    ILogger<CreateUserCommandHandler> logger)
    : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse>>
{
    public async Task<Result<CreateUserCommandResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var registerDto = mapper.Map<RegisterDto>(request);

        await userService.CreateUserAsync(registerDto, request.UserId, request.Scheme);

        logger.LogInformation("User {UserName} created successfully by admin {AdminId}",
            request.UserName, request.UserId);

        return Result<CreateUserCommandResponse>.Created(
            new("User created successfully and confirmation email sent."),
            "User created successfully.");
    }
}
