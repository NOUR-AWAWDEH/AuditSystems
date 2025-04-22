using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;

namespace AuditSystem.Application.Features.Processes.SubProcess.Get;

internal sealed class GetSubProcessByIdQueryHandler(
    ISubProcessService subProcessService) : 
    IRequestHandler<GetSubProcessByIdQuery, Result<GetSubProcessByIdQueryResponse>>
{
    public async Task<Result<GetSubProcessByIdQueryResponse>> Handle(GetSubProcessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await subProcessService.GetSubProcessByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetSubProcessByIdQueryResponse>.Success(new GetSubProcessByIdQueryResponse(result));
    }
}
