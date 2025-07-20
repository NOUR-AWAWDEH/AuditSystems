using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;
using Microsoft.Extensions.Logging;


namespace AuditSystem.Application.Features.Organization.SubLocation.Get.GetById;

internal sealed class GetSubLocationByIdQueryHandler(
    ISubLocationService subLocationService,
    ILogger<GetSubLocationByIdQueryHandler> logger) : 
    IRequestHandler<GetSubLocationByIdQuery,Result<GetSubLocationByIdQueryResponse>>
{
    public async Task<Result<GetSubLocationByIdQueryResponse>> Handle(GetSubLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var subLocation = await subLocationService.GetSubLocationByIdAsync(request.Id, request.UserId);
        
        logger.LogInformation("Sub-location retrieved successfully with ID: {SubLocationId} for user ID: {UserId}", request.Id, request.UserId);
        return Result<GetSubLocationByIdQueryResponse>.Success(new GetSubLocationByIdQueryResponse(
            subLocation), "Sub-location retrieved successfully");
    }
}
