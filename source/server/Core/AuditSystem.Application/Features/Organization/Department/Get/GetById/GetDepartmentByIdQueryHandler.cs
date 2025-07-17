using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;


namespace AuditSystem.Application.Features.Organization.Department.Get.GetById;

internal sealed class GetDepartmentByIdQueryHandler(
    IDepartmentService departmentService) 
    : IRequestHandler<GetDepartmentByIdQuery, Result<GetDepartmentByIdQueryResponse>>
{
    public async Task<Result<GetDepartmentByIdQueryResponse>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await departmentService.GetDepartmentByIdAsync(request.Id, request.UserId);
        return Result<GetDepartmentByIdQueryResponse>.Success(new GetDepartmentByIdQueryResponse(department), "Department retrived successfully");
    }
}