using MediatR;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Get.GetByUserId;


internal sealed class GetControlDesignsByUserIdQueryHandler(
    IControlDesignService controlDesignService,
    ILogger<GetControlDesignsByUserIdQuery> logger) : 
    IRequestHandler<GetControlDesignsByUserIdQuery, Result<GetControlDesignsByUserIdQueryResponse>>
{
    public async Task<Result<GetControlDesignsByUserIdQueryResponse>> Handle(
        GetControlDesignsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var controlDesigns = await controlDesignService.GetControlDesignsByUserIdAsync(request.UserId);

        logger.LogInformation("Control Designs were retrieved  by user with id: {UserId} successfully", request.UserId);
        return Result<GetControlDesignsByUserIdQueryResponse>.Success(new GetControlDesignsByUserIdQueryResponse(controlDesigns));
    }
}
