using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Common.Rating.Delete;

internal sealed class DeleteRateCommandHandler(
    IRateService rateService,
    ILogger<DeleteRateCommandHandler> logger) :
    IRequestHandler<DeleteRateCommand, Result>
{
    public async Task<Result> Handle(DeleteRateCommand request, CancellationToken cancellationToken)
    {
        await rateService.DeleteRateAsync(request.Id);
        logger.LogInformation("Deleting rate with ID {Id}", request.Id);
        return Result.SuccessWithMessage("Rate deleted successfully");
    }
}