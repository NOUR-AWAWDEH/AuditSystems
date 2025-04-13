using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Models.RiskControls;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Update;

internal sealed class UpdateRiskControlsCommandHandler(
    IRiskControlService riskControlService,
    IMapper mapper,
    ILogger<UpdateRiskControlsCommandHandler> logger)
    : IRequestHandler<UpdateRiskControlsCommand, Result>
{
    public async Task<Result> Handle(UpdateRiskControlsCommand request, CancellationToken cancellationToken)
    {
        var riskControlModel = mapper.Map<RiskControlsModel>(request);
        await riskControlService.UpdateRiskControlAsync(riskControlModel);
        logger.LogInformation("Risk control updated successfully");
        return Result.Success();
    }
}