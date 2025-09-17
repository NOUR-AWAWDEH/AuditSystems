using MediatR;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;

namespace AuditSystem.Application.Features.Risks.ExitControl.Get.GetById;


internal sealed class GetExitControlByIdQueryHandler(
    IExitControlService exitControlService,
    ILogger<GetExitControlByIdQuery> logger) : 
    IRequestHandler<GetExitControlByIdQuery, Result<GetExitControlByIdQueryResponse>>
{
    public async Task<Result<GetExitControlByIdQueryResponse>> Handle(GetExitControlByIdQuery request, CancellationToken cancellationToken)
    {
        var exitControl = await exitControlService.GetExitControlByIdAsync(request.Id, request.UserId);

        logger.LogInformation("Exit Control with id: {Id} was retrieved  by user with id: {UserId} successfully", request.Id, request.UserId);
        return Result<GetExitControlByIdQueryResponse>.Success(new GetExitControlByIdQueryResponse(exitControl));
    }
}
