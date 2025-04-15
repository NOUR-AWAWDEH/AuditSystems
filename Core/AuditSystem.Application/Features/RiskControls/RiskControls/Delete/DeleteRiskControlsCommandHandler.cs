using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Delete;

internal sealed class DeleteRiskControlsCommandHandler(
    IRiskControlService riskControlService,
    ILogger<DeleteRiskControlsCommandHandler> logger) :
    IRequestHandler<DeleteRiskControlsCommand, Result>
{
    public async Task<Result> Handle(DeleteRiskControlsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await riskControlService.DeleteRiskControlAsync(request.Id);
            logger.LogInformation("Risk control with id: {Id} was deleted", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting risk control with id: {Id}", request.Id);
            return Result.Error(ex.Message);  
        }  
    }
}