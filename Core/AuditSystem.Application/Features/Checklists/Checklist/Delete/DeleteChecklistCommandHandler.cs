using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using MediatR;
using Microsoft.Extensions.Logging;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Checklists.Checklist.Delete;

internal class DeleteChecklistCommandHandler(
    IChecklistService checklistService,
    ILogger<DeleteChecklistCommandHandler> logger) :
    IRequestHandler<DeleteChecklistCommand, Result>
{
    public async Task<Result> Handle(DeleteChecklistCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await checklistService.DeleteChecklistAsync(request.Id);
            logger.LogInformation("Deleting checklist with ID {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting checklist with ID {Id}", request.Id);
            return Result.Error(ex.Message);  
        } 
    }
}
