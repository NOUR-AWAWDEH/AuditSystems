using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

namespace AuditSystem.Application.Features.Risks.Commands.CreateRisk;

internal sealed class CreateRiskCommandHandler(
    IRiskService riskService,
    IMapper mapper,
    ILogger<CreateRiskCommandHandler> logger) :
    IRequestHandler<CreateRiskCommand, Result<CreateRiskCommandResponse>>
{
    public async Task<Result<CreateRiskCommandResponse>> Handle(CreateRiskCommand command, CancellationToken cancellationToken)
    {
        var riskModel = mapper.Map<RiskModel>(command);
        
        var riskId = await riskService.CreateRiskAsync(riskModel);
        logger.LogInformation("Risk with name {RiskName} has been created.", command.RiskName);
        return Result<CreateRiskCommandResponse>.Created(new CreateRiskCommandResponse(riskId));
    }
}