using MediatR;
using Microsoft.Extensions.Logging;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;

namespace AuditSystem.Application.Features.Processes.SubProcess.Delete;
internal sealed class DeleteSubProcessCommandHandler(
    ISubProcessService subProcessService,
    ILogger<DeleteSubProcessCommandHandler> logger) :
    IRequestHandler<DeleteSubProcessCommand, Result>
{
    public async Task<Result> Handle(DeleteSubProcessCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await subProcessService.DeleteSubProcessAsync(request.Id);
            logger.LogInformation("Sub process with id: {Id} was deleted", request.Id);
            return Result.Success();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting sub process with id: {Id}", request.Id);
            return Result.Error(ex.Message);
        }  
    }
}