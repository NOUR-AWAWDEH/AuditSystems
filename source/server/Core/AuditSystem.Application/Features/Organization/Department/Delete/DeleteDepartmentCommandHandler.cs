using Microsoft.Extensions.Logging;
using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

namespace AuditSystem.Application.Features.Organization.Department.Delete;

internal sealed class DeleteDepartmentCommandHandler(
    IDepartmentService departmentService,
    ILogger<DeleteDepartmentCommandHandler> logger) :
    IRequestHandler<DeleteDepartmentCommand, Result>
{
    public async Task<Result> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await departmentService.DeleteDepartmentAsync(request.Id);
            logger.LogInformation("Deleting department with ID {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting department with ID {Id}", request.Id);
            return Result.Error("An error occurred while deleting the department.");
        } 
    }
}