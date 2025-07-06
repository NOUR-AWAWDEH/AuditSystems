using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Get;

internal sealed class GetRiskFactorByIdQueryHandler(
    IRiskFactorService riskFactorService) : 
    IRequestHandler<GetRiskFactorByIdQuery, Result<GetRiskFactorByIdQueryResponse>>
{
    public async Task<Result<GetRiskFactorByIdQueryResponse>> Handle(GetRiskFactorByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await riskFactorService.GetRiskFactorByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetRiskFactorByIdQueryResponse>.Success(new GetRiskFactorByIdQueryResponse(result));
    }   
}