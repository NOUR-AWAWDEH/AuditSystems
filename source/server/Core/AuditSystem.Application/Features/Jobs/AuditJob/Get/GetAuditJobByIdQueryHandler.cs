using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Get;

internal sealed class GetAuditJobByIdQueryHandler( 
    IAuditJobService auditJobService) :
    IRequestHandler<GetAuditJobByIdQuery, Result<GetAuditJobByIdQueryResponse>>  
{
    public async Task<Result<GetAuditJobByIdQueryResponse>> Handle(GetAuditJobByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await auditJobService.GetAuditJobByIdAsync(request.Id);
        if (result == null) 
        {
            return Result.NotFound();  
        }

        return Result<GetAuditJobByIdQueryResponse>.Success(new GetAuditJobByIdQueryResponse(result));  
    }
}