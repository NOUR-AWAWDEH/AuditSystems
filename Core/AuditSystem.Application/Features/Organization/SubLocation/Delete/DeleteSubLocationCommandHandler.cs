using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
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
        try
        {
            await subLocationService.DeleteSubLocationAsync(request.Id);
            logger.LogInformation("Deleting sublocation with ID {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting sublocation with ID {Id}", request.Id);
            return Result.Error(ex.Message);
        }  
    }
}

