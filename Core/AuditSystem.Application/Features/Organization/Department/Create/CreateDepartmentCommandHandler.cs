using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.Department.Create;

internal sealed class CreateDepartmentCommandHandler(
    IDepartmentService departmentService,
    IMapper mapper,
    ILogger<CreateDepartmentCommandHandler> logger)
    : IRequestHandler<CreateDepartmentCommand, Result<CreateDepartmentCommandResponse>>
{
    public async Task<Result<CreateDepartmentCommandResponse>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var departmentModel = mapper.Map<DepartmentModel>(request);
        var departmentId = await departmentService.CreateDepartmentAsync(departmentModel);
        logger.LogInformation("Department with Name {DepartmentName} has been created.", request.Name);

        return Result<CreateDepartmentCommandResponse>.Created(new(departmentId));
    }
}
