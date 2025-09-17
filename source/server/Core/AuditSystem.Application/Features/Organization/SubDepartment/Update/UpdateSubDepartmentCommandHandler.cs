using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Update;

internal sealed class UpdateSubDepartmentCommandHandler(
    ISubDepartmentService subDepartmentService,
    IMapper mapper,
    ILogger<UpdateSubDepartmentCommandHandler> logger)
    : IRequestHandler<UpdateSubDepartmentCommand, Result>
{
    public async Task<Result> Handle(UpdateSubDepartmentCommand request, CancellationToken cancellationToken)
    {
        var subDepartmentModel = mapper.Map<SubDepartmentModel>(request);
        await subDepartmentService.UpdateSubDepartmentAsync(subDepartmentModel, request.UserId);
        
        logger.LogInformation("Sub-Department With ID: {SubDepartmentId} updated by User ID: {UserId} successfully", request.Id, request.UserId);
        return Result.SuccessWithMessage("Sub-Department updated successfully");
    }
}
