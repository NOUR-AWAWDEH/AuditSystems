using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
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
        try
        {
            await locationService.DeleteLocationAsync(request.Id);
            logger.LogInformation("Deleting location with ID {Id}", request.Id);
            return Result.Success();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting location with ID {Id}", request.Id);
            return Result.Error(ex.Message);
        }  
    }
}
