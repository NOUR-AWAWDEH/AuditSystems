using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Models.Skills;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Skills.SkillSet.Create;

internal sealed class CreateSkillSetCommandHandler(
    ISkillSetService skillSetService,
    IMapper mapper,
    ILogger<CreateSkillSetCommandHandler> logger)
    : IRequestHandler<CreateSkillSetCommand, Result<CreateSkillSetCommandResponse>>
{
    public async Task<Result<CreateSkillSetCommandResponse>> Handle(CreateSkillSetCommand request, CancellationToken cancellationToken)
    {
        var skillSetModel = mapper.Map<SkillSetModel>(request);
        var skillSetId = await skillSetService.CreateSkillSetAsync(skillSetModel);
        logger.LogInformation("SkillSet with Skill Id {SkillSetSkillId} has been created.", request.SkillId);

        return Result<CreateSkillSetCommandResponse>.Created(new CreateSkillSetCommandResponse(skillSetId));
    }
}
