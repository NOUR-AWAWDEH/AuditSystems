using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using Microsoft.Extensions.Logging;


namespace AuditSystem.Application.Features.Organization.Department.Get.GetById;

internal sealed class GetDepartmentByIdQueryHandler(
    IDepartmentService departmentService,
    ILogger<GetDepartmentByIdQueryHandler> logger) 
    : IRequestHandler<GetDepartmentByIdQuery, Result<GetDepartmentByIdQueryResponse>>
{
    public async Task<Result<GetDepartmentByIdQueryResponse>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await departmentService.GetDepartmentByIdAsync(request.Id, request.UserId);
        if (department == null)
            return Result.NotFound();
        
        logger.LogInformation("Department with id: {DepartmentId} has been retrived successfully by user with id: {UserId}", request.Id, request.UserId);
        return Result<GetDepartmentByIdQueryResponse>.Success(new GetDepartmentByIdQueryResponse(department), "Department retrived successfully");
    }
}