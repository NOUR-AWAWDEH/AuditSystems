using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.Risk.Delete;

internal sealed class DeleteRiskCommandHandler(
    IRiskService riskService,
    ILogger<DeleteRiskCommandHandler> logger) :
    IRequestHandler<DeleteRiskCommand, Result>
{
    public async Task<Result> Handle(DeleteRiskCommand request, CancellationToken cancellationToken)
    {
       try
       {
           await riskService.DeleteRiskAsync(request.Id);
           logger.LogInformation("Risk with id: {Id} was deleted", request.Id);
           return Result.Success();
       }
       catch (Exception ex)
       {
           logger.LogError(ex, "Error while deleting risk with id: {Id}", request.Id);
           return Result.Error("Error while deleting risk");  
       }
    }
}
