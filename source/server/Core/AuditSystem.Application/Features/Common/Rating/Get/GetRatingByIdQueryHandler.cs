using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;

namespace AuditSystem.Application.Features.Common.Rating.Get;

internal sealed class GetRatingByIdQueryHandler(
    IRateService ratingService) : 
    IRequestHandler<GetRatingByIdQuery, Result<GetRatingByIdQueryResponse>>
{
    public async Task<Result<GetRatingByIdQueryResponse>> Handle(GetRatingByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await ratingService.GetRatingByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetRatingByIdQueryResponse>.Success(new GetRatingByIdQueryResponse(result));
    }    
}
