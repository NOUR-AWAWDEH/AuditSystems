using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Delete;

internal sealed class DeleteAuditDomainCommandHandler(
    IAuditDomainService auditDomainService,
    ILogger<DeleteAuditDomainCommandHandler> logger) :
    IRequestHandler<DeleteAuditDomainCommand, Result>
{
    public async Task<Result> Handle(DeleteAuditDomainCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await auditDomainService.DeleteAuditDomainAsync(request.Id);
            logger.LogInformation("Audit domain with ID {Id} deleted successfully.", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting audit domain with ID {Id}.", request.Id);
            return Result.Error(ex.Message);
        }
        
    }
}
