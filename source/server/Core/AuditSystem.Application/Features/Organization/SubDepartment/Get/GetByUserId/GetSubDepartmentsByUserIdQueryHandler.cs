using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Get.GetByUserId;

internal sealed class GetSubDepartmentsByUserIdQueryHandler(
    ISubDepartmentService subDepartmentService,
    ILogger<GetSubDepartmentsByUserIdQueryHandler> logger) :
    IRequestHandler<GetSubDepartmentsByUserIdQuery, Result<GetSubDepartmentsByUserIdQueryResponse>>
{
    public async Task<Result<GetSubDepartmentsByUserIdQueryResponse>> Handle(
        GetSubDepartmentsByUserIdQuery request,
        CancellationToken cancellationToken)
    { 
        var subDepartments = await subDepartmentService.GetSubDepartmentsByUserIdAsync(request.UserId);
        logger.LogDebug("Sub-Departments retrieved successfully for user {UserId}", request.UserId);
        return Result<GetSubDepartmentsByUserIdQueryResponse>.Success(
            new GetSubDepartmentsByUserIdQueryResponse(subDepartments),
            "Sub-Departments retrieved successfully");
    }
}