using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubLocation.Create;

internal sealed class CreateSubLocationCommandHandler(
    ISubLocationService subLocationService,
    IMapper mapper,
    ILogger<CreateSubLocationCommandHandler> logger)
    : IRequestHandler<CreateSubLocationCommand, Result<CreateSubLocationCommandResponse>>
{
    public async Task<Result<CreateSubLocationCommandResponse>> Handle(CreateSubLocationCommand request, CancellationToken cancellationToken)
    {
        var subLocationModel = mapper.Map<SubLocationModel>(request);
        var subLocationId = await subLocationService.CreateSubLocationAsync(subLocationModel, request.UserId);
        
        logger.LogInformation("Sub-location created with ID: {SubLocationId} for user ID: {UserId}", subLocationId, request.UserId);
        return Result<CreateSubLocationCommandResponse>.Created(new CreateSubLocationCommandResponse(subLocationId), "Sub-location created.");
    }
}