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
    public async Task<Result<CreateRiskCommandResponse>> Handle(CreateRiskCommand request, CancellationToken cancellationToken)
    {
        var riskModel = mapper.Map<RiskModel>(request);
        var riskId = await riskService.CreateRiskAsync(riskModel, request.UserId);

        logger.LogInformation("Risk with name {Factor} has been created by user {UserId}.", request.Factor, request.UserId);
        return Result<CreateRiskCommandResponse>.Created(new CreateRiskCommandResponse(riskId), "Risk created.");
    }
}