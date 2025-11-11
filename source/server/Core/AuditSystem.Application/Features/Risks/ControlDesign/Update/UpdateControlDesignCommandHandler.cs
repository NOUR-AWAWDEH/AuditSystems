using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Update;

internal sealed class UpdateControlDesignCommandHandler(
    IControlDesignService controlDesignService,
    IMapper mapper,
    ILogger<UpdateControlDesignCommandHandler> logger)
    : IRequestHandler<UpdateControlDesignCommand, Result>
{
    public async Task<Result> Handle(
        UpdateControlDesignCommand request,
        CancellationToken cancellationToken)
    {
        var controlDesignModel = mapper.Map<ControlDesignModel>(request);
        await controlDesignService.UpdateControlDesignAsync(controlDesignModel, request.UserId);
        
        logger.LogInformation("Control Design updated successfully with ID: {ControlDesignId} by user Id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Control Design updated successfully");
    }
}
