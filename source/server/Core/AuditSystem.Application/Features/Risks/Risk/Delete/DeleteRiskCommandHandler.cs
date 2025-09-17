using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.Risk.Delete;

internal sealed class DeleteRiskCommandHandler(
    IRiskService riskService,
    ILogger<DeleteRiskCommandHandler> logger) :
    IRequestHandler<DeleteRiskCommand, Result>
{
    public async Task<Result> Handle(DeleteRiskCommand request, CancellationToken cancellationToken)
    {
       
        await riskService.DeleteRiskAsync(request.Id, request.UserId);

        logger.LogInformation("Risk with id: {Id} was deleted by user with id {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Risk deleted");
    }
}
