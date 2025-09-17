using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Models.Common;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Common.Rating.Create;

internal sealed class CreateRateCommandHandler(
    IRateService rateService,
    IMapper mapper,
    ILogger<CreateRateCommandHandler> logger)
    : IRequestHandler<CreateRateCommand, Result<CreateRateCommandResponse>>
{
    public async Task<Result<CreateRateCommandResponse>> Handle(
        CreateRateCommand request,
        CancellationToken cancellationToken)
    {
        var rateModel = mapper.Map<RateModel>(request);
        var rateId = await rateService.CreateRateAsync(rateModel);
        logger.LogInformation("Rate with Level {RateLevel} has been created.", request.Level);

        return Result<CreateRateCommandResponse>.Created(new(rateId), "Rate created.");
    }
}