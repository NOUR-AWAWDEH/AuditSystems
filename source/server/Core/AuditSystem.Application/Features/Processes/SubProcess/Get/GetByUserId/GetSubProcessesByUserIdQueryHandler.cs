using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Processes.SubProcess.Get.GetByUserId;

internal sealed class GetSubProcessesByUserIdQueryHandler(
    ISubProcessService subProcessService,
    ILogger<GetSubProcessesByUserIdQueryHandler> logger) : 

    IRequestHandler<GetSubProcessesByUserIdQuery, Result<GetSubProcessesByUserIdQueryResponse>>
{
    public async Task<Result<GetSubProcessesByUserIdQueryResponse>> Handle(
        GetSubProcessesByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var subProcesses = await subProcessService.GetSubProcessesByUserIdAsync(request.UserId);

        logger.LogInformation("Sub processes with user id: {UserId} was retrieved", request.UserId);
        return Result<GetSubProcessesByUserIdQueryResponse>.Success(new GetSubProcessesByUserIdQueryResponse(subProcesses.ToList()), "Sub Processes retrieved successfully");
    }
}