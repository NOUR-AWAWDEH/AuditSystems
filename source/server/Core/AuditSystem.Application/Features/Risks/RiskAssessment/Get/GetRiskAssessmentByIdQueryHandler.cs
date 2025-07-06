using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Get;

internal sealed class GetRiskAssessmentByIdQueryHandler(
    IRiskAssessmentService riskAssessmentService) : 
    IRequestHandler<GetRiskAssessmentByIdQuery, Result<GetRiskAssessmentByIdQueryResponse>>
{
    public async Task<Result<GetRiskAssessmentByIdQueryResponse>> Handle(GetRiskAssessmentByIdQuery request, CancellationToken cancellationToken)
    {
       var result = await riskAssessmentService.GetRiskAssessmentByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetRiskAssessmentByIdQueryResponse>.Success(new GetRiskAssessmentByIdQueryResponse(result));
    }
}