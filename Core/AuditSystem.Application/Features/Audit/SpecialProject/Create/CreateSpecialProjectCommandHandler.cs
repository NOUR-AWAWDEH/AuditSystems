using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Create;

internal sealed class CreateSpecialProjectCommandHandler(
    ISpecialProjectService specialProjectService,
    IMapper mapper,
    ILogger<CreateSpecialProjectCommandHandler> logger)
    : IRequestHandler<CreateSpecialProjectCommand, Result<CreateSpecialProjectCommandResponse>>
{
    public async Task<Result<CreateSpecialProjectCommandResponse>> Handle(CreateSpecialProjectCommand request, CancellationToken cancellationToken)
    {
        var specialProjectModel = mapper.Map<SpecialProjectModel>(request);
        var specialProjectId = await specialProjectService.CreateSpecialProjectAsync(specialProjectModel);
        logger.LogInformation("Special Project with Project Name {SpecialProjectName} has been created.", request.ProjectName);

        return Result<CreateSpecialProjectCommandResponse>.Created(new CreateSpecialProjectCommandResponse(specialProjectId));
    }
}
