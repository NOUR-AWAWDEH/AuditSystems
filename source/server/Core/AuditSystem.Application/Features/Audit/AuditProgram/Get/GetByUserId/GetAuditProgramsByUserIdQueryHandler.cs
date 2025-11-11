using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Get.GetByUserId;

internal sealed class GetAuditProgramsByUserIdQueryHandler(
    IAuditProgramService auditProgramService,
    ILogger<GetAuditProgramsByUserIdQueryHandler> logger ) : 
    IRequestHandler<GetAuditProgramsByUserIdQuery, Result<GetAuditProgramsByUserIdQueryResponse>>
{
    public async Task<Result<GetAuditProgramsByUserIdQueryResponse>> Handle(    
        GetAuditProgramsByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        var auditPrograms = await auditProgramService.GetAuditProgramsByUserIdAsync(request.UserId);
        logger.LogDebug("Audit Programs retrieved successfully for user {UserId}", request.UserId);
        return Result<GetAuditProgramsByUserIdQueryResponse>.Success(
            new(auditPrograms),
            "Audit Programs retrieved successfully");
    }
}
