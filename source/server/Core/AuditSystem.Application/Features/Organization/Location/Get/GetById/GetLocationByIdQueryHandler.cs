using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.Location.Get.GetById;

internal sealed class GetLocationByIdQueryHandler(
    ILocationService locationService,
    ILogger<GetLocationByIdQueryHandler> logger) : 
    IRequestHandler<GetLocationByIdQuery, Result<GetLocationByIdQueryResponse>>
{
    public async Task<Result<GetLocationByIdQueryResponse>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var location = await locationService.GetLocationByIdAsync(request.Id, request.UserId);   
        
        logger.LogInformation("Location with id: {Id} was retrieved by user with id: {UserId}", request.Id, request.UserId);
        return Result<GetLocationByIdQueryResponse>.Success(new GetLocationByIdQueryResponse(location), "Location retrieved successfully");
    }
}
