using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.Risk.Create;

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