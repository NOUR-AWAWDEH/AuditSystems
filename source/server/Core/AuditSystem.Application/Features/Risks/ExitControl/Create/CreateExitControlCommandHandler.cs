using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.ExitControl.Create;

internal sealed class CreateExitControlCommandHandler(
    IExitControlService exitControlService,
    IMapper mapper,
    ILogger<CreateExitControlCommandHandler> logger) :
    IRequestHandler<CreateExitControlCommand, Result<CreateExitControlCommandResponse>>
{
    public async Task<Result<CreateExitControlCommandResponse>> Handle(CreateExitControlCommand request, CancellationToken cancellationToken)
    {
        var exitControlModel = mapper.Map<ExitControlModel>(request);
        var exitControlId = await exitControlService.CreateExitControlAsync(exitControlModel, request.UserId);

        logger.LogInformation("Exit Control with description {Description} has been created by user {UserId}.", request.Description, request.UserId);
        return Result<CreateExitControlCommandResponse>.Created(new CreateExitControlCommandResponse(exitControlId), "Exit Control created.");
    }
}