using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Models.Compliance;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Update;

internal sealed class UpdateComplianceChecklistCommandHandler(
    IComplianceChecklistService complianceChecklistService,
    IMapper mapper,
    ILogger<UpdateComplianceChecklistCommandHandler> logger)
    : IRequestHandler<UpdateComplianceChecklistCommand, Result>
{
    public async Task<Result> Handle(UpdateComplianceChecklistCommand request, CancellationToken cancellationToken)
    {
        var complianceChecklistModel = mapper.Map<ComplianceChecklistModel>(request);
        await complianceChecklistService.UpdateComplianceChecklistAsync(complianceChecklistModel);

        logger.LogInformation("Compliance checklist updated successfully");
        return Result.Success();
    }
}
