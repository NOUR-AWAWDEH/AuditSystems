using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Common.Rating.Delete;

internal sealed class DeleteRatingCommandHandler(
    IRatingService ratingService,
    ILogger<DeleteRatingCommandHandler> logger) :
    IRequestHandler<DeleteRatingCommand, Result>
{
    public async Task<Result> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await ratingService.DeleteRatingAsync(request.Id);
            logger.LogInformation("Deleting rating with ID {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting rating with ID {Id}", request.Id);
            return Result.Error(ex.Message); 
        }
    }
}