using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Get;

internal sealed class GetAuditDomainByIdQueryHandler(
    IAuditDomainService auditDomainService)
    : IRequestHandler<GetAuditDomainByIdQuery, Result<GetAuditDomainByIdQueryResponse>>
{
    public async Task<Result<GetAuditDomainByIdQueryResponse>> Handle(GetAuditDomainByIdQuery request, CancellationToken cancellationToken)
    {
       var auditDomain = await auditDomainService.GetAuditDomainByIdAsync(request.Id);
        if(auditDomain is null)
        {
            return Result.NotFound();
        }
        
        return Result<GetAuditDomainByIdQueryResponse>.Success(new GetAuditDomainByIdQueryResponse(auditDomain));
    }
}