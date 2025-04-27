using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Get;

internal sealed class GetSubDepartmentByIdQueryHandler(
    ISubDepartmentService subDepartmentService) : 
    IRequestHandler<GetSubDepartmentByIdQuery, Result<GetSubDepartmentByIdQueryResponse>>
{
    public async Task<Result<GetSubDepartmentByIdQueryResponse>> Handle( GetSubDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await subDepartmentService.GetSubDepartmentByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetSubDepartmentByIdQueryResponse>.Success(new GetSubDepartmentByIdQueryResponse(result));
    }
}
