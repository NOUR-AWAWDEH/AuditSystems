using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using MediatR;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Delete;

internal sealed class DeleteSpecialProjectCommandHandler(
    ISpecialProjectService specialProjectService,
    ILogger<DeleteSpecialProjectCommandHandler> logger) 
    : IRequestHandler<DeleteSpecialProjectCommand, Result>
{
    public async Task<Result> Handle(DeleteSpecialProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await specialProjectService.DeleteSpecialProjectAsync(request.Id);
            logger.LogInformation("Deleting special project with ID {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting special project with ID {Id}", request.Id);
            return Result.Error(ex.Message); 
        } 
    }
}
