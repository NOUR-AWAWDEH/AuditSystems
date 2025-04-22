using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

namespace AuditSystem.Application.Features.Organization.Department.Get;

internal sealed class GetDepartmentByIdQueryHandler(
    IDepartmentService departmentService) 
    : IRequestHandler<GetDepartmentByIdQuery, Result<GetDepartmentByIdQueryResponse>>
{
    public async Task<Result<GetDepartmentByIdQueryResponse>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await departmentService.GetDepartmentByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetDepartmentByIdQueryResponse>.Success(new GetDepartmentByIdQueryResponse(result));
    }
}