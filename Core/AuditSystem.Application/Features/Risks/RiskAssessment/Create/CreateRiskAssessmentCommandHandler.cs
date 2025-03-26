using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Create;

internal sealed class CreateRiskAssessmentCommandHandler(
    IRiskAssessmentService riskAssessmentService,
    IMapper mapper,
    ILogger<CreateRiskAssessmentCommandHandler> logger) 
    : IRequestHandler<CreateRiskAssessmentCommand, Result<CreateRiskAssessmentCommandResponse>>
{
    public async Task<Result<CreateRiskAssessmentCommandResponse>> Handle(CreateRiskAssessmentCommand request, CancellationToken cancellationToken)
    {
        var riskAssessmentModel = mapper.Map<RiskAssessmentModel>(request);
        var riskAssessmentId = await riskAssessmentService.CreateRiskAssessmentAsync(riskAssessmentModel);
        logger.LogInformation("Risk Assessment with Business Objective {RiskAssessmentBusinessObjective} has been created.", request.BusinessObjective);

        return Result<CreateRiskAssessmentCommandResponse>.Created(new CreateRiskAssessmentCommandResponse(riskAssessmentId));
    }
}
