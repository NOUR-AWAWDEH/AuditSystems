using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;

namespace AuditSystem.Application.Features.Risks.ExitControl.Delete;

internal sealed class DeleteExitControlCommandHandler(
    IExitControlService exitControlService,
    ILogger<DeleteExitControlCommandHandler> logger) :
    IRequestHandler<DeleteExitControlCommand, Result>
{
    public async Task<Result> Handle(DeleteExitControlCommand request, CancellationToken cancellationToken)
    {
        await exitControlService.DeleteExitControlAsync(request.Id, request.UserId);

        logger.LogInformation("Exit Control with id: {Id} was deleted by user with id {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Exit Control deleted");
    }
}
