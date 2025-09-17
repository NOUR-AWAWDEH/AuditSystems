using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
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
        
        await subDepartmentService.DeleteSubDepartmentAsync(request.Id, request.UserId);
        
        logger.LogInformation("Sub-Department with ID: {Id} deleted by User ID: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Sub-Department was deleted. Successfully.");  
    }
}