using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Get;

internal sealed class GetRiskControlMatrixByIdQueryHandler(
    IRiskControlMatrixService riskControlMatrixService) : 
    IRequestHandler<GetRiskControlMatrixByIdQuery, Result<GetRiskControlMatrixByIdQueryResponse>>
{
    public async Task<Result<GetRiskControlMatrixByIdQueryResponse>> Handle(GetRiskControlMatrixByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await riskControlMatrixService.GetRiskControlMatrixByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetRiskControlMatrixByIdQueryResponse>.Success(new GetRiskControlMatrixByIdQueryResponse(result));
    }
}
