using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Create;

internal sealed class CreateRiskFactorCommandHandler(
    IRiskFactorService riskFactorService,
    IMapper mapper,
    ILogger<CreateRiskFactorCommandHandler> logger)
    : IRequestHandler<CreateRiskFactorCommand, Result<CreateRiskFactorCommandResponse>>
{
    public async Task<Result<CreateRiskFactorCommandResponse>> Handle(CreateRiskFactorCommand request, CancellationToken cancellationToken)
    {
        var riskFactorModel = mapper.Map<RiskFactorModel>(request);
        var riskFactorId = await riskFactorService.CreateRiskFactorAsync(riskFactorModel);
        logger.LogInformation("Risk Factor with Factor {RiskFactor}", request.Factor);

        return Result<CreateRiskFactorCommandResponse>.Created(new CreateRiskFactorCommandResponse(riskFactorId));
    }
}
