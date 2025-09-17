using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
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
        var riskControlMatrixId = await riskControlMatrixService.CreateRiskControlMatrixAsync(riskControlMatrixModel, request.UserId);
        
        logger.LogInformation("Risk Control Matrix has been created by UserID {UserId}.", request.UserId);
        return Result<CreateRiskControlMatrixCommandResponse>.Created(new CreateRiskControlMatrixCommandResponse(riskControlMatrixId), "Risk Control Matrix created.");
    }
}
