using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Models.Compliance;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Create;

internal sealed class CreateComplianceChecklistCommandHandler(
    IComplianceChecklistService complianceChecklistService,
    IMapper mapper,
    ILogger<CreateComplianceChecklistCommandHandler> logger) : IRequestHandler<CreateComplianceChecklistCommand, Result<CreateComplianceChecklistCommandResponse>>
{
    public async Task<Result<CreateComplianceChecklistCommandResponse>> Handle(CreateComplianceChecklistCommand request, CancellationToken cancellationToken)
    {
        var complianceChecklistModel = mapper.Map<ComplianceChecklistModel>(request);
        var complianceChecklistId = await complianceChecklistService.CreateComplianceChecklistAsync(complianceChecklistModel);
        logger.LogInformation("Compliance Checklist with Area {ComplianceChecklistArea} has been created.", request.Area);

        return Result<CreateComplianceChecklistCommandResponse>.Created(new CreateComplianceChecklistCommandResponse(complianceChecklistId));
    }
}
