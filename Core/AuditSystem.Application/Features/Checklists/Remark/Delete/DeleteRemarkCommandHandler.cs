using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using Microsoft.Extensions.Logging;
using Ardalis.Result;
using MediatR; 

namespace AuditSystem.Application.Features.Checklists.Remark.Delete;

internal sealed class DeleteRemarkCommandHandler(
    IRemarkService remarkService,
    ILogger<DeleteRemarkCommandHandler> logger) :
    IRequestHandler<DeleteRemarkCommand, Result>
{
    public async Task<Result> Handle(DeleteRemarkCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await remarkService.DeleteRemarkAsync(request.Id);
            logger.LogInformation("Deleting remark with ID {Id}", request.Id);
            return Result.SuccessWithMessage("Remark deleted successfully"); 
        } 
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting remark with ID {Id}", request.Id);
            return Result.Error(ex.Message);
        }
    }
}