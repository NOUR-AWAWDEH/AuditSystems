using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Get;

internal sealed class GetAuditUniverseByIdQueryHandler(
    IAuditUniverseService auditUniverseService ) :
    IRequestHandler<GetAuditUniverseByIdQuery, Result<GetAuditUniverseByIdQueryResponse>>
{
    public async Task<Result<GetAuditUniverseByIdQueryResponse>> Handle(GetAuditUniverseByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await auditUniverseService.GetAuditUniverseByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetAuditUniverseByIdQueryResponse>.Success(new GetAuditUniverseByIdQueryResponse(result));
    }
}
