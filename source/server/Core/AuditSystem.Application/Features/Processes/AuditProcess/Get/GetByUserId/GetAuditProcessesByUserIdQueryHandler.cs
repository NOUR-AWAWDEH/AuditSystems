using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Get.GetByUserId;

internal sealed class GetAuditProcessesByUserIdQueryHandler(
    IAuditProcessService auditProcessService,
    ILogger<GetAuditProcessesByUserIdQueryHandler> logger) : 

    IRequestHandler<GetAuditProcessesByUserIdQuery, Result<GetAuditProcessesByUserIdQueryResponse>>
{
    public async Task<Result<GetAuditProcessesByUserIdQueryResponse>> Handle(
        GetAuditProcessesByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var auditProcesses = await auditProcessService.GetAuditProcessesByUserIdAsync(request.UserId);
        if (auditProcesses == null || auditProcesses.Count == 0)
            return Result<GetAuditProcessesByUserIdQueryResponse>.NotFound();

        logger.LogInformation("Audit processes with user id: {UserId} was retrieved", request.UserId);
        return Result<GetAuditProcessesByUserIdQueryResponse>.Success(new GetAuditProcessesByUserIdQueryResponse(auditProcesses.ToList()), "Audit Processes retrieved successfully");
    }
}