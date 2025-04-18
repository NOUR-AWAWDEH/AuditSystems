using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.Location.Create;

internal sealed class CreateLocationCommandHandler(
    ILocationService locationService,
    IMapper mapper,
    ILogger<CreateLocationCommandHandler> logger)
    : IRequestHandler<CreateLocationCommand, Result<CreateLocationCommandResponse>>
{
    public async Task<Result<CreateLocationCommandResponse>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var locationModel = mapper.Map<LocationModel>(request);
        var locationId = await locationService.CreateLocationAsync(locationModel);
        logger.LogInformation("Location with Name {LocationName} has been created.", request.Name);

        return Result<CreateLocationCommandResponse>.Created(new(locationId));
    }
}
