using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Models.Skills;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Skills.Skill.Create;

internal sealed class CreateSkillCommandHander(
    ISkillService skillService,
    IMapper mapper,
    ILogger<CreateSkillCommandHander> logger)
    : IRequestHandler<CreateSkillCommand, Result<CreateSkillCommandResponse>>
{
    public async Task<Result<CreateSkillCommandResponse>> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        var skillModel = mapper.Map<SkillModel>(request);
        var skillId = await skillService.CreateSkillAsync(skillModel);
        logger.LogInformation("Skill with Name {SkillName} has been created.", request.Name);

        return Result<CreateSkillCommandResponse>.Created(new CreateSkillCommandResponse(skillId));
    }
}
