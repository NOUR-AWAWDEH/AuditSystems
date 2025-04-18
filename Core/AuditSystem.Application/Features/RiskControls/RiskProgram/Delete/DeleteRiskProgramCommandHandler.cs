using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Delete;

internal sealed class DeleteRiskProgramCommandHandler(
    IRiskProgramService riskProgramService,
    ILogger<DeleteRiskProgramCommandHandler> logger) :
    IRequestHandler<DeleteRiskProgramCommand, Result>
{
    public async Task<Result> Handle(DeleteRiskProgramCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await riskProgramService.DeleteRiskProgramAsync(request.Id);
            logger.LogInformation("Risk program with id: {Id} was deleted", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting risk program with id: {Id}", request.Id);
            return Result.Error("Error deleting risk program");
        }
    }
}