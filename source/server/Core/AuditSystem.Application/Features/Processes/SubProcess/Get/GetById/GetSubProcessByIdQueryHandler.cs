using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Processes.SubProcess.Get.GetById;

internal sealed class GetSubProcessByIdQueryHandler(
    ISubProcessService subProcessService,
    ILogger<GetSubProcessByIdQueryHandler> logger) : 
    IRequestHandler<GetSubProcessByIdQuery, Result<GetSubProcessByIdQueryResponse>>
{
    public async Task<Result<GetSubProcessByIdQueryResponse>> Handle(GetSubProcessByIdQuery request, CancellationToken cancellationToken)
    {
        var subProcess = await subProcessService.GetSubProcessByIdAsync(request.Id, request.UserId);

        logger.LogInformation("Sub process with id: {Id} was retrieved by User id: {UserId}", request.Id, request.UserId);
        return Result<GetSubProcessByIdQueryResponse>.Success(new GetSubProcessByIdQueryResponse(subProcess));
    }
}
