using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Models.Common;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Common.Rating.Update;

internal sealed class UpdateRatingCommandHandler(
    IRatingService ratingService,
    IMapper mapper,
    ILogger<UpdateRatingCommandHandler> logger)
    : IRequestHandler<UpdateRatingCommand, Result>
{
    public async Task<Result> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        var ratingModel = mapper.Map<RatingModel>(request);
        await ratingService.UpdateRatingAsync(ratingModel);

        logger.LogInformation("Rating updated successfully");
        return Result.Success();
    }
}
