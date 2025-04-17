using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Create;

internal sealed class CreateSubDepartmentCommandHandler(
    ISubDepartmentService subDepartmentService,
    IMapper mapper,
    ILogger<CreateSubDepartmentCommandHandler> logger)
    : IRequestHandler<CreateSubDepartmentCommand, Result<CreateSubDepartmentCommandResponse>>
{
    public async Task<Result<CreateSubDepartmentCommandResponse>> Handle(CreateSubDepartmentCommand request, CancellationToken cancellationToken)
    {
        var subDepartmentModel = mapper.Map<SubDepartmentModel>(request);
        var subDepartmentId = await subDepartmentService.CreateSubDepartmentAsync(subDepartmentModel);
        logger.LogInformation("SubDepartment with Name {SubDepartmentName} has been created.", request.Name);

        return Result<CreateSubDepartmentCommandResponse>.Created(new(subDepartmentId));
    }
}
