using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Delete;

internal sealed class DeleteComplianceChecklistCommandHandler(
    IComplianceChecklistService complianceChecklistService,
    ILogger<DeleteComplianceChecklistCommandHandler> logger)
    : IRequestHandler<DeleteComplianceChecklistCommand, Result>
{
    public async Task<Result> Handle(DeleteComplianceChecklistCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await complianceChecklistService.DeleteComplianceChecklistAsync(request.Id);
            logger.LogInformation("Deleting compliance checklist with ID {Id}", request.Id);
            return Result.Success();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting compliance checklist with ID {Id}", request.Id);
            return Result.Error("An error occurred while deleting the compliance checklist.");
        } 
    }
}

