using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;

namespace AuditSystem.Application.Features.Common.Rating.Get;

internal sealed class GetRateByIdQueryHandler(
    IRateService rateService) : 
    IRequestHandler<GetRateByIdQuery, Result<GetRateByIdQueryResponse>>
{
    public async Task<Result<GetRateByIdQueryResponse>> Handle(GetRateByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await rateService.GetRateByIdAsync(request.Id);

        return Result<GetRateByIdQueryResponse>.Success(new GetRateByIdQueryResponse(result));
    }    
}
