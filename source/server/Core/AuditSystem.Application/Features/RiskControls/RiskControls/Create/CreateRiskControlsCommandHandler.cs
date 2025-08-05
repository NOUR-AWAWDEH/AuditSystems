using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
using AuditSystem.Contract.Models.RiskControls;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Create;

internal sealed class CreateRiskControlsCommandHandler(
    IRiskControlService riskControlService,
    IMapper mapper,
    ILogger<CreateRiskControlsCommandHandler> logger)
    : IRequestHandler<CreateRiskControlsCommand, Result<CreateRiskControlsCommandResponse>>
{
    public async Task<Result<CreateRiskControlsCommandResponse>> Handle(CreateRiskControlsCommand request, CancellationToken cancellationToken)
    {
        var riskControlModel = mapper.Map<RiskControlsModel>(request);
        var riskControlId = await riskControlService.CreateRiskControlAsync(riskControlModel);
        logger.LogInformation("Risk Control with Description {RiskControlDescription} has been created.", request.Description);

        return Result<CreateRiskControlsCommandResponse>.Created(new CreateRiskControlsCommandResponse(riskControlId), "Risk Control created.");
    }
}