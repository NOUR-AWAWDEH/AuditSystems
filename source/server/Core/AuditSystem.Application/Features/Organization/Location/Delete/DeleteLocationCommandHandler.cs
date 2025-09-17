using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.Location.Delete;

internal sealed class DeleteLocationCommandHandler(
    ILocationService locationService,
    ILogger<DeleteLocationCommandHandler> logger) :
    IRequestHandler<DeleteLocationCommand, Result>
{
    public async Task<Result> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        await locationService.DeleteLocationAsync(request.Id, request.UserId);
        
        logger.LogInformation("Location with id: {Id} was delete by user with id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Location deleted.");  
    }
}
