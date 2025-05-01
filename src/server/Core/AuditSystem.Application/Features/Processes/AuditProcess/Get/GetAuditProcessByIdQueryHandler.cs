using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Get;

internal sealed class GetAuditProcessByIdQueryHandler(
    IAuditProcessService auditProcessService) : 
    IRequestHandler<GetAuditProcessByIdQuery, Result<GetAuditProcessByIdQueryResponse>>
{
    public async Task<Result<GetAuditProcessByIdQueryResponse>> Handle(GetAuditProcessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await auditProcessService.GetAuditProcessByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetAuditProcessByIdQueryResponse>.Success(new GetAuditProcessByIdQueryResponse(result));
    }
}
