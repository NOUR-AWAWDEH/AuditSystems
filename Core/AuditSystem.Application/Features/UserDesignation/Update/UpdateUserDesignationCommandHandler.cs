using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.UserDesignationServices;
using AuditSystem.Contract.Models.UserDesignation;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.UserDesignation.Update;

internal sealed class UpdateUserDesignationCommandHandler(
    IUserDesignationService userDesignationService,
    IMapper mapper,
    ILogger<UpdateUserDesignationCommandHandler> logger)
    : IRequestHandler<UpdateUserDesignationCommand, Result>
{
    public async Task<Result> Handle(UpdateUserDesignationCommand request, CancellationToken cancellationToken)
    {
        var userDesignationModel = mapper.Map<UserDesignationModel>(request);
        await userDesignationService.UpdateUserDesignationAsync(userDesignationModel);
        
        logger.LogInformation("User designation with ID {Id} updated successfully.", request.Id);
        return Result.Success();
    }
}