using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;


namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Delete;

internal sealed class DeleteRiskControlMatrixCommandHandler(
    IRiskControlMatrixService riskControlMatrixService,
    ILogger<DeleteRiskControlMatrixCommandHandler> logger) :
    IRequestHandler<DeleteRiskControlMatrixCommand, Result>
{
    public async Task<Result> Handle(DeleteRiskControlMatrixCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await riskControlMatrixService.DeleteRiskControlMatrixAsync(request.Id);
            logger.LogInformation("Risk control matrix with id: {Id} was deleted", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting risk control matrix with id: {Id}", request.Id);
            return Result.Error(ex.Message);  
        }  
    }
}