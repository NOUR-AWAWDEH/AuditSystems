using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Create;

internal sealed class CreateAuditProgramCommandHandler(
    IAuditProgramService auditProgramService,
    IMapper mapper,
    ILogger<CreateAuditProgramCommandHandler> logger)
    : IRequestHandler<CreateAuditProgramCommand, Result<CreateAuditProgramCommandResponse>>
{
    public async Task<Result<CreateAuditProgramCommandResponse>> Handle(CreateAuditProgramCommand request, CancellationToken cancellationToken)
    {
        var auditProgramModel = mapper.Map<AuditProgramModel>(request);
        var auditProgramId = await auditProgramService.CreateAuditProgramAsync(auditProgramModel, request.UserId);
        
        logger.LogInformation("Audit Program with Description {AuditProgramDescription} has been created by user with id: {UserId}", request.Description, request.UserId);
        return Result<CreateAuditProgramCommandResponse>.Created(new(auditProgramId), "Audit Program has been created.");
    }
}