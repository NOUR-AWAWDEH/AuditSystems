using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Delete;

internal sealed class DeleteRiskAssessmentCommandHandler(
    IRiskAssessmentService riskAssessmentService,
    ILogger<DeleteRiskAssessmentCommandHandler> logger) :
    IRequestHandler<DeleteRiskAssessmentCommand, Result>
{
    public async Task<Result> Handle(DeleteRiskAssessmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await riskAssessmentService.DeleteRiskAssessmentAsync(request.Id);
            logger.LogInformation("Risk assessment with id: {Id} was deleted", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting risk assessment with id: {Id}", request.Id);
            return Result.Error(ex.Message);
        }
    }
}