using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Delete
{
    internal sealed class DeleteRiskFactorCommandHandler(
        IRiskProgramService riskProgramService,
        ILogger<DeleteRiskFactorCommandHandler> logger ) :
        IRequestHandler<DeleteRiskFactorCommand, Result>
    {
        public async Task<Result> Handle(DeleteRiskFactorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await riskProgramService.DeleteRiskProgramAsync(request.Id);
                logger.LogInformation("Risk program with id: {Id} was deleted", request.Id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while deleting risk program with id: {Id}", request.Id);
                return Result.Error(ex.Message);  
            }  
        }
    }
}
