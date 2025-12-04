using Microsoft.Extensions.Logging;
using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;

namespace AuditSystem.Application.Features.Organization.Department.Delete;

internal sealed class DeleteDepartmentCommandHandler(
    IDepartmentService departmentService,
    ILogger<DeleteDepartmentCommandHandler> logger) :
    IRequestHandler<DeleteDepartmentCommand, Result>
{
    public async Task<Result> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        await departmentService.DeleteDepartmentAsync(request.Id, request.UserId);
        logger.LogInformation("Department with ID {Id} has been deleted by user with id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Department deleted successfully");
    }
}