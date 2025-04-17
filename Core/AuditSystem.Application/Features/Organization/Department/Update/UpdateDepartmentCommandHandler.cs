using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
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
        await departmentService.UpdateDepartmentAsync(departmentModel);

        logger.LogInformation("Department updated successfully");
        return Result.Success();
    }
}
