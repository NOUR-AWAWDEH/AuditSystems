using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Models.RiskControls;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Update;

internal sealed class UpdateRiskControlMatrixCommandHandler(
    IRiskControlMatrixService riskControlMatrixService,
    IMapper mapper,
    ILogger<UpdateRiskControlMatrixCommandHandler> logger)
    : IRequestHandler<UpdateRiskControlMatrixCommand, Result>
{
    public async Task<Result> Handle(UpdateRiskControlMatrixCommand request, CancellationToken cancellationToken)
    {
        var riskControlMatrixModel = mapper.Map<RiskControlMatrixModel>(request);
        await riskControlMatrixService.UpdateRiskControlMatrixAsync(riskControlMatrixModel);

        logger.LogInformation("Risk control matrix updated successfully");
        return Result.Success();
    }
}