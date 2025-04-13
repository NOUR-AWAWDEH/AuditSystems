using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Models.RiskControls;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Update;

internal sealed class UpdateRiskProgramCommandHandler(
    IRiskProgramService riskProgramService,
    IMapper mapper,
    ILogger<UpdateRiskProgramCommandHandler> logger)
    : IRequestHandler<UpdateRiskProgramCommand, Result>
{
    public async Task<Result> Handle(UpdateRiskProgramCommand request, CancellationToken cancellationToken)
    {
        var riskProgramModel = mapper.Map<RiskProgramModel>(request);
        await riskProgramService.UpdateRiskProgramAsync(riskProgramModel);

        logger.LogInformation("Risk program updated successfully");
        return Result.Success();
    }
}
