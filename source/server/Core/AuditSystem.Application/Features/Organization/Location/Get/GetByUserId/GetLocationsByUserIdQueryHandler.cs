using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
using AuditSystem.Contract.Models.Organization;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.Location.Get.GetByUserId;

internal sealed class GetLocationsByUserIdQueryHandler(
    ILocationService locationService,
    ILogger<GetLocationsByUserIdQueryHandler> logger) :
    IRequestHandler<GetLocationsByUserIdQuery, Result<GetLocationsByUserIdQueryResponse>>
{
    public async Task<Result<GetLocationsByUserIdQueryResponse>> Handle(GetLocationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var locations = await locationService.GetLocationsByUserIdAsync(request.UserId);
        
        logger.LogInformation("Locations with user id: {UserId} were retrieved successfully.", request.UserId);
        return Result<GetLocationsByUserIdQueryResponse>.Success(new(locations), "Locations retrieved successfully.");
    }
}