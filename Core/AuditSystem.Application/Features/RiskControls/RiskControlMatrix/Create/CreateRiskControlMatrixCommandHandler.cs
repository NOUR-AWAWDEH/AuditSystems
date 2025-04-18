using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Models.RiskControls;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Create;

internal sealed class CreateRiskControlMatrixCommandHandler(
    IRiskControlMatrixService riskControlMatrixService,
    IMapper mapper,
    ILogger<CreateRiskControlMatrixCommandHandler> logger)
    : IRequestHandler<CreateRiskControlMatrixCommand, Result<CreateRiskControlMatrixCommandResponse>>
{
    public async Task<Result<CreateRiskControlMatrixCommandResponse>> Handle(CreateRiskControlMatrixCommand request, CancellationToken cancellationToken)
    {
        var riskControlMatrixModel = mapper.Map<RiskControlMatrixModel>(request);
        var riskControlMatrixId = await riskControlMatrixService.CreateRiskControlMatrixAsync(riskControlMatrixModel);
        logger.LogInformation("Risk Control Matrix with SubProcessID {RiskControlMatrixImpact} has been created.", request.SubProcessId);

        return Result<CreateRiskControlMatrixCommandResponse>.Created(new CreateRiskControlMatrixCommandResponse(riskControlMatrixId));
    }
}
