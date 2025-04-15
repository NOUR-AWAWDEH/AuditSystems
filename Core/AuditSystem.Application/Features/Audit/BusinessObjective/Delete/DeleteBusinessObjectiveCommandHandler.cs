using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using MediatR;
using Microsoft.Extensions.Logging;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Delete;

internal sealed class DeleteBusinessObjectiveCommandHandler(
    IBusinessObjectiveService businessObjectiveService,
    ILogger<DeleteBusinessObjectiveCommandHandler> logger) 
    : IRequestHandler<DeleteBusinessObjectiveCommand, Result>
{
    public async Task<Result> Handle(DeleteBusinessObjectiveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await businessObjectiveService.DeleteBusinessObjectiveAsync(request.Id);
            logger.LogInformation("Deleting business objective with ID {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting business objective with ID {Id}", request.Id);
            return Result.Error(ex.Message);
        }
    }
}
