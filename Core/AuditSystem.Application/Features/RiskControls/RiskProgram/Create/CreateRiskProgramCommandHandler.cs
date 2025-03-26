using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Models.RiskControls;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Create;

internal sealed class CreateRiskProgramCommandHandler(
    IRiskProgramService riskProgramService,
    IMapper mapper,
    ILogger<CreateRiskProgramCommandHandler> logger)
    : IRequestHandler<CreateRiskProgramCommand, Result<CreateRiskProgramCommandResponse>>
{
    public async Task<Result<CreateRiskProgramCommandResponse>> Handle(CreateRiskProgramCommand request, CancellationToken cancellationToken)
    {
        var riskProgramModel = mapper.Map<RiskProgramModel>(request);
        var riskProgramId = await riskProgramService.CreateRiskProgramAsync(riskProgramModel);
        logger.LogInformation("Risk Program with Name {RiskProgramName} has been created.", request.Name);

        return Result<CreateRiskProgramCommandResponse>.Created(new CreateRiskProgramCommandResponse(riskProgramId));
    }
}
