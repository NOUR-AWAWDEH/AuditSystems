using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Get;

internal sealed class GetAuditUniverseObjectiveByIdQueryHandler(
    IAuditUniverseObjectiveService auditUniverseObjectiveService) :
    IRequestHandler<GetAuditUniverseObjectiveByIdQuery, Result<GetAuditUniverseObjectiveByIdQueryResponse>>
{
    public async Task<Result<GetAuditUniverseObjectiveByIdQueryResponse>> Handle( GetAuditUniverseObjectiveByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await auditUniverseObjectiveService.GetAuditUniverseObjectiveByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetAuditUniverseObjectiveByIdQueryResponse>.Success(new GetAuditUniverseObjectiveByIdQueryResponse(result));
    }
}
