using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Location.Get.GetByUserId;

internal sealed class GetLocationsByUserIdQueryHandler(
    ILocationService locationService) :
    IRequestHandler<GetLocationsByUserIdQuery, Result<GetLocationsByUserIdQueryResponse>>
{
    public async Task<Result<GetLocationsByUserIdQueryResponse>> Handle(GetLocationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var locations = await locationService.GetLocationsByUserIdAsync(request.UserId);
        
        if (locations == null || locations.Count == 0)
        {
            return Result<GetLocationsByUserIdQueryResponse>.Success(new GetLocationsByUserIdQueryResponse(new List<LocationModel>()));
        }

        return Result<GetLocationsByUserIdQueryResponse>.Success(new GetLocationsByUserIdQueryResponse(locations.ToList()));
    }
}