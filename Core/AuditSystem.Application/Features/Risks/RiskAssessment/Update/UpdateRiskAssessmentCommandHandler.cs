using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Update;

internal sealed class UpdateRiskAssessmentCommandHandler(
    IRiskAssessmentService riskAssessmentService,
    IMapper mapper,
    ILogger<UpdateRiskAssessmentCommandHandler> logger)
    : IRequestHandler<UpdateRiskAssessmentCommand, Result>
{
    public async Task<Result> Handle(UpdateRiskAssessmentCommand request, CancellationToken cancellationToken)
    {
        var riskAssessmentModel = mapper.Map<RiskAssessmentModel>(request);
        await riskAssessmentService.UpdateRiskAssessmentAsync(riskAssessmentModel);
        
        logger.LogInformation("Risk assessment updated successfully");
        return Result.Success();
    }
}
