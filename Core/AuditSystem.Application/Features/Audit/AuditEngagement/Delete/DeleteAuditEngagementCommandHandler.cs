using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Delete;

internal sealed class DeleteAuditEngagementCommandHandler (
    IAuditEngagementService auditEngagementService,
    ILogger<DeleteAuditEngagementCommandHandler> logger) : 
    IRequestHandler<DeleteAuditEngagementCommand, Result>
{
    public async Task<Result> Handle(DeleteAuditEngagementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await auditEngagementService.DeleteAuditEngagementAsync(request.Id);
            logger.LogInformation("Deleting audit engagement with ID {Id}", request.Id);
            return Result.Success();

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting audit engagement with ID {Id}", request.Id);
            return Result.Error(ex.Message);
        }
    }
}

