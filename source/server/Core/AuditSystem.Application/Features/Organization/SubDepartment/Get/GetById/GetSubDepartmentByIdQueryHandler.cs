using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Get.GetById;


internal sealed class GetSubDepartmentByIdQueryHandler(
    ISubDepartmentService subDepartmentService) : 
    IRequestHandler<GetSubDepartmentByIdQuery, Result<GetSubDepartmentByIdQueryResponse>>
{
    public async Task<Result<GetSubDepartmentByIdQueryResponse>> Handle( GetSubDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var subDepartment = await subDepartmentService.GetSubDepartmentByIdAsync(request.Id, request.UserId);
        return Result<GetSubDepartmentByIdQueryResponse>.Success(new GetSubDepartmentByIdQueryResponse(
            subDepartment), 
            "Sub-Department retrieved successfully");
    }
}
