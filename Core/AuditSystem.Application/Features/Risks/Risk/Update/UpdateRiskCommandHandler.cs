using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.Risk.Update;

internal sealed class UpdateRiskCommandHandler(
    IRiskService riskService,
    IMapper mapper,
    ILogger<UpdateRiskCommandHandler> logger)
    : IRequestHandler<UpdateRiskCommand, Result>
{
    public async Task<Result> Handle(UpdateRiskCommand request, CancellationToken cancellationToken)
    {
       var riskModel = mapper.Map<RiskModel>(request);
        await riskService.UpdateRiskAsync(riskModel);
        
        logger.LogInformation("Risk updated successfully");
        return Result.Success();
    }
}
