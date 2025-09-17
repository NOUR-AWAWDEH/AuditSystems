using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;

using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.Department.Update;

internal sealed class UpdateDepartmentCommandHandler(
    IDepartmentService departmentService,
    IMapper mapper,
    ILogger<UpdateDepartmentCommandHandler> logger)
    : IRequestHandler<UpdateDepartmentCommand, Result>
{
    public async Task<Result> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var departmentModel = mapper.Map<DepartmentModel>(request);
        await departmentService.UpdateDepartmentAsync(departmentModel, request.UserId);

        logger.LogInformation("Department with ID: {DepartmentId} updated by User Id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Department updated successfully");
    }
}
