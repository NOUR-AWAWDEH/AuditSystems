using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Update;

public sealed class UpdateAuditProgramCommandHandler(
    IAuditProgramService auditProgramService,
    IMapper mapper,
    ILogger<UpdateAuditProgramCommandHandler> logger)
    : IRequestHandler<UpdateAuditProgramCommand, Result>
{
    public async Task<Result> Handle(UpdateAuditProgramCommand request, CancellationToken cancellationToken)
    {
        var auditProgramModel = mapper.Map<AuditProgramModel>(request);
        await auditProgramService.UpdateAuditProgramAsync(auditProgramModel, request.UserId);

        logger.LogInformation("Audit Program with ID: {AuditProgramId} updated by User Id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Audit Program updated successfully");
    }
}
