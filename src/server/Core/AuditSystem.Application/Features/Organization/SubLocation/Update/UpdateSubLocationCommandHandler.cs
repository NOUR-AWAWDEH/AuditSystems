using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
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
        var subLocationModel = new SubLocationModel
        {
            Id = request.Id,
            Name = request.Name,
            Address = request.Address,
            LocationId = request.LocationId
        };

        await subLocationService.UpdateSubLocationAsync(subLocationModel, request.UserId);
        return Result.SuccessWithMessage("Sub-location updated successfully");
    }
}