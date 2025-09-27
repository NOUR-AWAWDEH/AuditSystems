using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Create;

internal sealed class CreateControlDesignCommandHandler(
    IControlDesignService controlDesignService,
    IMapper mapper,
    ILogger<CreateControlDesignCommandHandler> logger) :
    IRequestHandler<CreateControlDesignCommand, Result<CreateControlDesignCommandResponse>>
{
    public async Task<Result<CreateControlDesignCommandResponse>> Handle(CreateControlDesignCommand request, CancellationToken cancellationToken)
    {
        var controlDesignModel = mapper.Map<ControlDesignModel>(request);
        var controlDesignId = await controlDesignService.CreateControlDesignAsync(controlDesignModel, request.UserId);

        logger.LogInformation("Control Design with description {Description} has been created by user {UserId}.", request.Description, request.UserId);
        return Result<CreateControlDesignCommandResponse>.Created(new CreateControlDesignCommandResponse(controlDesignId), "Control Design created.");
    }
}