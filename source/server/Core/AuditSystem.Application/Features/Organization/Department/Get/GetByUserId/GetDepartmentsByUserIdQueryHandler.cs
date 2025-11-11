using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.Department.Get.GetByUserId;

internal sealed class GetDepartmentsByUserIdQueryHandler(
    IDepartmentService departmentService,
    ILogger<GetDepartmentsByUserIdQueryHandler> logger ) : 
    IRequestHandler<GetDepartmentsByUserIdQuery, Result<GetDepartmentsByUserIdQueryResponse>>
{
    public async Task<Result<GetDepartmentsByUserIdQueryResponse>> Handle(
        GetDepartmentsByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        var departments = await departmentService.GetDepartmentsByUserIdAsync(request.UserId);
        logger.LogDebug("Departments retrieved successfully for user {UserId}", request.UserId);
        return Result<GetDepartmentsByUserIdQueryResponse>.Success(
            new(departments),
            "Departments retrieved successfully");
    }
}
