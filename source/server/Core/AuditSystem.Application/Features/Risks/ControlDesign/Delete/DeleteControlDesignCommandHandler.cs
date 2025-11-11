using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Delete;

internal sealed class DeleteControlDesignCommandHandler(
    IControlDesignService controlDesignService,
    ILogger<DeleteControlDesignCommandHandler> logger) :
    IRequestHandler<DeleteControlDesignCommand, Result>
{
    public async Task<Result> Handle(DeleteControlDesignCommand request, CancellationToken cancellationToken)
    {
        await controlDesignService.DeleteControlDesignAsync(request.Id, request.UserId);
        logger.LogInformation("Control Design with id: {Id} was deleted by user with id {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Control Design deleted");
    }
}
