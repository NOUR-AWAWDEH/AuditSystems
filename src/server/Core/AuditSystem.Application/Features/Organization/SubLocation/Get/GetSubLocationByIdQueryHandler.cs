using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get;

internal sealed class GetSubLocationByIdQueryHandler(
    ISubLocationService subLocationService) : 
    IRequestHandler<GetSubLocationByIdQuery,Result<GetSubLocationByIdQueryResponse>>
{
    public async Task<Result<GetSubLocationByIdQueryResponse>> Handle(GetSubLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var subLocation = await subLocationService.GetSubLocationByIdAsync(request.Id, request.UserId);
        return Result<GetSubLocationByIdQueryResponse>.Success(new GetSubLocationByIdQueryResponse(subLocation), "Sub-location retrieved successfully");
    }
}
