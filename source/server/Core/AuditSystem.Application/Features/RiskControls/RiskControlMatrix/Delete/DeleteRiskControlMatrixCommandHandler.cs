using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;


namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Delete;

internal sealed class DeleteRiskControlMatrixCommandHandler(
    IRiskControlMatrixService riskControlMatrixService,
    ILogger<DeleteRiskControlMatrixCommandHandler> logger) :
    IRequestHandler<DeleteRiskControlMatrixCommand, Result>
{
    public async Task<Result> Handle(DeleteRiskControlMatrixCommand request, CancellationToken cancellationToken)
    {
        await riskControlMatrixService.DeleteRiskControlMatrixAsync(request.Id, request.UserId);

        logger.LogInformation("Risk control matrix with id: {Id} was deleted by UserId: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Risk control matrix was deleted");
    }
}