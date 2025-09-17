using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;


namespace AuditSystem.Application.Features.Audit.AuditProgram.Get.GetById;

internal sealed class GetAuditProgramByIdQueryHandler(
    IAuditProgramService auditProgramService,
    ILogger<GetAuditProgramByIdQueryHandler> logger) 
    : IRequestHandler<GetAuditProgramByIdQuery, Result<GetAuditProgramByIdQueryResponse>>
{
    public async Task<Result<GetAuditProgramByIdQueryResponse>> Handle(GetAuditProgramByIdQuery request, CancellationToken cancellationToken)
    {
        var auditProgram = await auditProgramService.GetAuditProgramByIdAsync(request.Id, request.UserId);
        
        logger.LogInformation("Audit Program with id: {AuditProgramId} has been retrieved successfully by user with id: {UserId}", request.Id, request.UserId);
        return Result<GetAuditProgramByIdQueryResponse>.Success(new GetAuditProgramByIdQueryResponse(auditProgram), "Audit Program retrieved successfully");
    }
}