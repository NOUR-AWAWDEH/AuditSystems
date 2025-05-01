using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Get;

internal sealed class GetAuditEngagementByIdQueryHandler(
    IAuditEngagementService auditEngagementService) 
    : IRequestHandler<GetAuditEngagementByIdQuery, Result<GetAuditEngagementByIdQueryResponse>>
{
    public async Task<Result<GetAuditEngagementByIdQueryResponse>> Handle(GetAuditEngagementByIdQuery request, CancellationToken cancellationToken)
    {
       var audit = await auditEngagementService.GetAuditEngagementByIdAsync(request.Id);

        if (audit is null)
        {
            return Result.NotFound(); 
        }

        return Result<GetAuditEngagementByIdQueryResponse>.Success(new GetAuditEngagementByIdQueryResponse(audit));
    }
}
