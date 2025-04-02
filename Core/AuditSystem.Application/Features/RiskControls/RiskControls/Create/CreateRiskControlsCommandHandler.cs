using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Models.RiskControls;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Create;

internal sealed class CreateRiskControlsCommandHandler(
    IRiskControlMatrixService riskControlMatrixService,
    IMapper mapper,
    ILogger<CreateRiskControlsCommandHandler> logger)
    : IRequestHandler<CreateRiskControlsCommand, Result<CreateRiskControlsCommandResponse>>
{
    public async Task<Result<CreateRiskControlsCommandResponse>> Handle(CreateRiskControlsCommand request, CancellationToken cancellationToken)
    {
        var riskControlMatrixModel = mapper.Map<RiskControlMatrixModel>(request);
        var riskControlId = await riskControlMatrixService.CreateRiskControlMatrixAsync(riskControlMatrixModel);
        logger.LogInformation("Risk Control with  RiskId {RiskControlRiskId} has been created.", request.RiskId);

        return Result<CreateRiskControlsCommandResponse>.Created(new CreateRiskControlsCommandResponse(riskControlId));
    }
}