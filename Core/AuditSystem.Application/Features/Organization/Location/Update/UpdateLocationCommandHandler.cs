using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.Location.Update;

internal sealed class UpdateLocationCommandHandler(
    ILocationService locationService,
    IMapper mapper,
    ILogger<UpdateLocationCommandHandler> logger)
    : IRequestHandler<UpdateLocationCommand, Result>
{
    public async Task<Result> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var locationModel = mapper.Map<LocationModel>(request);
        await locationService.UpdateLocationAsync(locationModel);
        logger.LogInformation("Location updated successfully");
        return Result.Success();
    }
}
