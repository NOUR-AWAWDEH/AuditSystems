using MediatR;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Get.GetById;


internal sealed class GetControlDesignByIdQueryHandler(
    IControlDesignService controlDesignService,
    ILogger<GetControlDesignByIdQuery> logger) : 
    IRequestHandler<GetControlDesignByIdQuery, Result<GetControlDesignByIdQueryResponse>>
{
    public async Task<Result<GetControlDesignByIdQueryResponse>> Handle(GetControlDesignByIdQuery request, CancellationToken cancellationToken)
    {
        var controlDesign = await controlDesignService.GetControlDesignByIdAsync(request.Id, request.UserId);

        logger.LogInformation("Control Design with id: {Id} was retrieved  by user with id: {UserId} successfully", request.Id, request.UserId);
        return Result<GetControlDesignByIdQueryResponse>.Success(new GetControlDesignByIdQueryResponse(controlDesign));
    }
}
