using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubLocation.Update;

internal sealed class UpdateSubLocationCommandHandler(
    ISubLocationService subLocationService,
    IMapper mapper,
    ILogger<UpdateSubLocationCommandHandler> logger)
    : IRequestHandler<UpdateSubLocationCommand, Result>
{
    public async Task<Result> Handle(UpdateSubLocationCommand request, CancellationToken cancellationToken)
    {
        var subLocationModel = mapper.Map<SubLocationModel>(request);
        await subLocationService.UpdateSubLocationAsync(subLocationModel, request.UserId);
        
        logger.LogInformation("Sub-location with ID: {SubLocationId} updated by User ID: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Sub-location updated successfully");
    }
}