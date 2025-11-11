using MediatR;
using Microsoft.Extensions.Logging;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;


namespace AuditSystem.Application.Features.Processes.SubProcess.Delete;
internal sealed class DeleteSubProcessCommandHandler(
    ISubProcessService subProcessService,
    ILogger<DeleteSubProcessCommandHandler> logger) :
    IRequestHandler<DeleteSubProcessCommand, Result>
{
    public async Task<Result> Handle(DeleteSubProcessCommand request, CancellationToken cancellationToken)
    {
        await subProcessService.DeleteSubProcessAsync(request.Id, request.UserId);
        
        logger.LogInformation("Sub process with id: {Id} was deleted by User id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Sub process deleted"); 
    }
}