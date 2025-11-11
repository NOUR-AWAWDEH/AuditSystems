using Microsoft.Extensions.Logging;
using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Delete;

internal sealed class DeleteAuditProgramCommandHandler(
    IAuditProgramService auditProgramService,
    ILogger<DeleteAuditProgramCommandHandler> logger) :
    IRequestHandler<DeleteAuditProgramCommand, Result>
{
    public async Task<Result> Handle(DeleteAuditProgramCommand request, CancellationToken cancellationToken)
    {
        await auditProgramService.DeleteAuditProgramAsync(request.Id, request.UserId);
        logger.LogInformation("Audit Program with ID {Id} has been deleted by user with id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Audit Program deleted successfully");
    }
}