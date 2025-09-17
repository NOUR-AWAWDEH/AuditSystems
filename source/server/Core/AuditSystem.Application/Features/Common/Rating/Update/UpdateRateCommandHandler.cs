using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Models.Common;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Common.Rating.Update;

internal sealed class UpdateRateCommandHandler(
    IRateService rateService,
    IMapper mapper,
    ILogger<UpdateRateCommandHandler> logger)
    : IRequestHandler<UpdateRateCommand, Result>
{
    public async Task<Result> Handle(UpdateRateCommand request, CancellationToken cancellationToken)
    {
        var rateModel = mapper.Map<RateModel>(request);
        await rateService.UpdateRateAsync(rateModel);

        logger.LogInformation("Rate updated successfully");
        return Result.SuccessWithMessage("Rate updated successfully");
    }
}
