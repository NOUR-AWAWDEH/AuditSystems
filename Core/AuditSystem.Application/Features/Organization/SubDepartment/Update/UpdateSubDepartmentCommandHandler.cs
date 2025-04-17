using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Update;

internal sealed class UpdateSubDepartmentCommandHandler(
    ISubDepartmentService departmentService,
    IMapper mapper,
    ILogger<UpdateSubDepartmentCommandHandler> logger)
    : IRequestHandler<UpdateSubDepartmentCommand, Result>
{
    public async Task<Result> Handle(UpdateSubDepartmentCommand request, CancellationToken cancellationToken)
    {
        var subDepartmentModel = mapper.Map<SubDepartmentModel>(request);
        await departmentService.UpdateSubDepartmentAsync(subDepartmentModel);
        
        logger.LogInformation("SubDepartment updated successfully");
        return Result.Success();
    }
}
