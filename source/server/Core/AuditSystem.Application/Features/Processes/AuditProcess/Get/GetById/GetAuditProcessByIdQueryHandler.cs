using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Get.GetById;

internal sealed class GetAuditProcessByIdQueryHandler(
    IAuditProcessService auditProcessService,
    ILogger<GetAuditProcessByIdQueryHandler> logger) : 
    IRequestHandler<GetAuditProcessByIdQuery, Result<GetAuditProcessByIdQueryResponse>>
{
    public async Task<Result<GetAuditProcessByIdQueryResponse>> Handle(GetAuditProcessByIdQuery request, CancellationToken cancellationToken)
    {
        var auditProcess = await auditProcessService.GetAuditProcessByIdAsync(request.Id, request.UserId);
       
        logger.LogInformation("Audit process with id: {Id} was retrieved by user with id: {UserId}", request.Id, request.UserId);
        return Result<GetAuditProcessByIdQueryResponse>.Success(new GetAuditProcessByIdQueryResponse(auditProcess));
    }
}
