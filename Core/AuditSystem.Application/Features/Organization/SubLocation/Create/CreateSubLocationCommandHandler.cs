using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
using AuditSystem.Contract.Models.Organisation;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organisation.SubLocation.Create;

internal sealed class CreateSubLocationCommandHandler(
    ISubLocationService subLocationService,
    IMapper mapper,
    ILogger<CreateSubLocationCommandHandler> logger)
    : IRequestHandler<CreateSubLocationCommand, Result<CreateSubLocationCommandResponse>>
{
    public async Task<Result<CreateSubLocationCommandResponse>> Handle(CreateSubLocationCommand request, CancellationToken cancellationToken)
    {
        var subLocationModel = mapper.Map<SubLocationModel>(request);
        var subLocationId = await subLocationService.CreateSubLocationAsync(subLocationModel);
        logger.LogInformation("SubLocation with Name {SubLocationName} has been created.", request.Name);

        return Result<CreateSubLocationCommandResponse>.Created(new CreateSubLocationCommandResponse(subLocationId));
    }
}