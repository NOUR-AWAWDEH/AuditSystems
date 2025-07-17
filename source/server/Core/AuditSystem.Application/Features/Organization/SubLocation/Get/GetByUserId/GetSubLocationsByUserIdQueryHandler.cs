using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get.GetByUserId;

internal sealed class GetSubLocationsByUserIdQueryHandler(
    ISubLocationService subLocationService,
    ILogger<GetSubLocationsByUserIdQueryHandler> logger) : 
    IRequestHandler<GetSubLocationsByUserIdQuery, Result<GetSubLocationsByUserIdQueryResponse>>
{
    public async Task<Result<GetSubLocationsByUserIdQueryResponse>> Handle(
        GetSubLocationsByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        var subLocations = await subLocationService.GetSubLocationsByUserIdAsync(request.UserId);
        logger.LogDebug("Sub-locations retrieved successfully for user {UserId}", request.UserId);
        return Result<GetSubLocationsByUserIdQueryResponse>.Success(
            new GetSubLocationsByUserIdQueryResponse(subLocations), 
            "Sub-locations retrieved successfully");
    }
}