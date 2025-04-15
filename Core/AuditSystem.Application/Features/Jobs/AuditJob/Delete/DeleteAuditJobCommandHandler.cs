using Microsoft.Extensions.Logging;
using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Delete;

internal sealed class DeleteAuditJobCommandHandler(
    IAuditJobService auditJobService,
    ILogger<DeleteAuditJobCommandHandler> logger) :
    IRequestHandler<DeleteAuditJobCommand, Result>
{
    public async Task<Result> Handle(DeleteAuditJobCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await auditJobService.DeleteAuditJobAsync(request.Id);
            logger.LogInformation("Deleting audit job with ID {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting audit job with ID {Id}", request.Id);
            return Result.Error(ex.Message);  
        } 
    }
}