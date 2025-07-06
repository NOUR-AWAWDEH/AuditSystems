using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

namespace AuditSystem.Application.Features.Audit.Objective.Get;

internal sealed class GetObjectiveByIdQueryHandler(
    IObjectiveService objectiveService) : 
    IRequestHandler<GetObjectiveByIdQuery, Result<GetObjectiveByIdQueryResponse>>
{
    public async Task<Result<GetObjectiveByIdQueryResponse>> Handle(GetObjectiveByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await objectiveService.GetObjectiveByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetObjectiveByIdQueryResponse>.Success(new GetObjectiveByIdQueryResponse(result));
    }
}
