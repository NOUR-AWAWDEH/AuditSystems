using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.UserManagementServices;
using AuditSystem.Contract.Models.Users;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.UserDesignation.Create;

internal sealed class CreateUserDesignationCommandHandler(
    IUserDesignationService _userDesignationService,
    IMapper _mapper,
    ILogger<CreateUserDesignationCommandHandler> _logger)
    : IRequestHandler<CreateUserDesignationCommand, Result<CreateUserDesignationCommandResponse>>
{

    public async Task<Result<CreateUserDesignationCommandResponse>> Handle(CreateUserDesignationCommand request, CancellationToken cancellationToken)
    {
        var userDesignationModel = _mapper.Map<UserDesignationModel>(request);
        var userDesignationId = await _userDesignationService.CreateUserDesignationAsync(userDesignationModel);
        _logger.LogInformation("User Designation with Designation {Designation} has been created.", request.Designation);
        return Result<CreateUserDesignationCommandResponse>.Created(new CreateUserDesignationCommandResponse(userDesignationId));
    }
}
