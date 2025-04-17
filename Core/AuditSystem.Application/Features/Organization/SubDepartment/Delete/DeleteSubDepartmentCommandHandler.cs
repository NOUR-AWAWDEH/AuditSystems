using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Delete;

internal sealed class DeleteSubDepartmentCommandHandler(
    ISubDepartmentService subDepartmentService,
    ILogger<DeleteSubDepartmentCommandHandler> logger) :
    IRequestHandler<DeleteSubDepartmentCommand, Result>
{
    public async Task<Result> Handle(DeleteSubDepartmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await subDepartmentService.DeleteSubDepartmentAsync(request.Id);
            logger.LogInformation("Deleting subdepartment with ID {Id}", request.Id);
            return Result.Success();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting subdepartment with ID {Id}", request.Id);
            return Result.Error(ex.Message);
        }  
    }
}