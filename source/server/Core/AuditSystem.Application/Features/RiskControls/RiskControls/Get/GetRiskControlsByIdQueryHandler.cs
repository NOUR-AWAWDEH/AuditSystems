using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Get;

internal sealed class GetRiskControlsByIdQueryHandler(
    IRiskControlService riskControlService) : 
    IRequestHandler<GetRiskControlsByIdQuery, Result<GetRiskControlsByIdQueryResponse>>
{
    public async Task<Result<GetRiskControlsByIdQueryResponse>> Handle(GetRiskControlsByIdQuery request, CancellationToken cancellationToken)
    {
       var result = await riskControlService.GetRiskControlByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetRiskControlsByIdQueryResponse>.Success(new GetRiskControlsByIdQueryResponse(result));
    }
}