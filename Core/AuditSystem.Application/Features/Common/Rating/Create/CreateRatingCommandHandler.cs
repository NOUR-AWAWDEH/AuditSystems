using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Models.Common;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace AuditSystem.Application.Features.Common.Rating.Create;

internal sealed class CreateRatingCommandHandler(
    IRatingService ratingService,
    IMapper mapper,
    ILogger<CreateRatingCommandHandler> logger)
    : IRequestHandler<CreateRatingCommand, Result<CreateRatingCommandResponse>>
{
    public async Task<Result<CreateRatingCommandResponse>> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
    {
        var ratingModel = mapper.Map<RatingModel>(request);
        var ratingId = await ratingService.CreateRatingAsync(ratingModel);
        logger.LogInformation("Rating with Level {RatingLevel} has been created.", request.Level);

        return Result<CreateRatingCommandResponse>.Created(new(ratingId));
    }
}