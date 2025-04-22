using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Get;

internal sealed class GetBusinessObjectiveByIdQueryHandler(
    IBusinessObjectiveService businessObjectiveService) : 
    IRequestHandler<GetBusinessObjectiveByIdQuery, Result<GetBusinessObjectiveByIdQueryResponse>>
{
    public async Task<Result<GetBusinessObjectiveByIdQueryResponse>> Handle(GetBusinessObjectiveByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await businessObjectiveService.GetBusinessObjectiveByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetBusinessObjectiveByIdQueryResponse>.Success(new GetBusinessObjectiveByIdQueryResponse(result));
    }
}
