using MediatR;
using Ardalis.Result;

using AuditSystem.Contract.Interfaces.ModelServices.TasksServices;

namespace AuditSystem.Application.Features.Tasks.Get;

internal sealed class GetTaskManagementByIdQueryHandler(
    ITaskManagementService  taskManagementService) :
    IRequestHandler<GetTaskManagementByIdQuery, Result<GetTaskManagementByIdQueryResponse>>
{
    public async Task<Result<GetTaskManagementByIdQueryResponse>> Handle(GetTaskManagementByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await taskManagementService.GetTaskByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetTaskManagementByIdQueryResponse>.Success(new GetTaskManagementByIdQueryResponse(result)); 
    }
}
