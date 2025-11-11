using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.ExitControl.Update;

internal sealed class UpdateExitControlCommandHandler(
    IExitControlService exitControlService,
    IMapper mapper,
    ILogger<UpdateExitControlCommandHandler> logger)
    : IRequestHandler<UpdateExitControlCommand, Result>
{
    public async Task<Result> Handle(
        UpdateExitControlCommand request,
        CancellationToken cancellationToken)
    {
        var exitControlModel = mapper.Map<ExitControlModel>(request);
        await exitControlService.UpdateExitControlAsync(exitControlModel, request.UserId);
        
        logger.LogInformation("Exit Control updated successfully with ID: {ExitControlId} by user Id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Exit Control updated successfully");
    }
}
