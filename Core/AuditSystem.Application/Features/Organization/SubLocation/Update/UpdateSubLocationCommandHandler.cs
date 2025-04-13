using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
using AuditSystem.Contract.Models.Organisation;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organisation.SubLocation.Update;

internal sealed class UpdateSubLocationCommandHandler(
    ISubLocationService subLocationService,
    IMapper mapper,
    ILogger<UpdateSubLocationCommandHandler> logger)
    : IRequestHandler<UpdateSubLocationCommand, Result>
{
    public async Task<Result> Handle(UpdateSubLocationCommand request, CancellationToken cancellationToken)
    {
        var subLocationModel = mapper.Map<SubLocationModel>(request);
        await subLocationService.UpdateSubLocationAsync(subLocationModel);

        logger.LogInformation("Sub-location updated successfully");
        return Result.Success();
    }
}