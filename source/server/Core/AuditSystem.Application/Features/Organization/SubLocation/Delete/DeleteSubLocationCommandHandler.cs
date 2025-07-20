using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubLocation.Delete;

internal sealed class DeleteSubLocationCommandHandler(
    ISubLocationService subLocationService,
    ILogger<DeleteSubLocationCommandHandler> logger) :
    IRequestHandler<DeleteSubLocationCommand, Result>
{
    public async Task<Result> Handle(DeleteSubLocationCommand request, CancellationToken cancellationToken)
    {
        await subLocationService.DeleteSubLocationAsync(request.Id, request.UserId);
        
        logger.LogInformation("Sub-location with ID: {SubLocationId} deleted by User ID: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Sub-location was deleted Sucessfully");
    }
}

