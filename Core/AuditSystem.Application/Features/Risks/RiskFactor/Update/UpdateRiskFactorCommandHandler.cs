using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Update;

internal sealed class UpdateRiskFactorCommandHandler(
    IRiskFactorService riskFactorService,
    IMapper mapper,
    ILogger<UpdateRiskFactorCommandHandler> logger)
    : IRequestHandler<UpdateRiskFactorCommand, Result>
{
    public async Task<Result> Handle(UpdateRiskFactorCommand request, CancellationToken cancellationToken)
    {
        var riskFactorModel = mapper.Map<RiskFactorModel>(request);
        await riskFactorService.UpdateRiskFactorAsync(riskFactorModel);
        
        logger.LogInformation("Risk factor updated successfully");
        return Result.Success();
    }
}
