using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;

namespace AuditSystem.Application.Features.Risks.ExitControl.Get.GetByUserId;


internal sealed class GetExitControlsByUserIdQueryHandler(
    IExitControlService exitControlService,
    ILogger<GetExitControlsByUserIdQuery> logger) : 
    IRequestHandler<GetExitControlsByUserIdQuery, Result<GetExitControlsByUserIdQueryResponse>>
{
    public async Task<Result<GetExitControlsByUserIdQueryResponse>> Handle(
        GetExitControlsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var exitControls = await exitControlService.GetExitControlsByUserIdAsync(request.UserId);

        logger.LogInformation("Exit Controls were retrieved  by user with id: {UserId} successfully", request.UserId);
        return Result<GetExitControlsByUserIdQueryResponse>.Success(new GetExitControlsByUserIdQueryResponse(exitControls));
    }
}
