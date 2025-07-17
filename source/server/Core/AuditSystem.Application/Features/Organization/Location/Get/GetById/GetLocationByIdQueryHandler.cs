using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;

namespace AuditSystem.Application.Features.Organization.Location.Get.GetById;

internal sealed class GetLocationByIdQueryHandler(
    ILocationService locationService) : 
    IRequestHandler<GetLocationByIdQuery, Result<GetLocationByIdQueryResponse>>
{
    public async Task<Result<GetLocationByIdQueryResponse>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await locationService.GetLocationByIdAsync(request.Id, request.UserId);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetLocationByIdQueryResponse>.Success(new GetLocationByIdQueryResponse(result));
    }
}
