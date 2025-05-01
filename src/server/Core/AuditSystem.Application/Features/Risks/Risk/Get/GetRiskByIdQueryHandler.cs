using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

namespace AuditSystem.Application.Features.Risks.Risk.Get;

internal sealed class GetRiskByIdQueryHandler(
    IRiskService riskService) : 
    IRequestHandler<GetRiskByIdQuery, Result<GetRiskByIdQueryResponse>>
{
    public async Task<Result<GetRiskByIdQueryResponse>> Handle(GetRiskByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await riskService.GetRiskByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetRiskByIdQueryResponse>.Success(new GetRiskByIdQueryResponse(result));
    }
}
