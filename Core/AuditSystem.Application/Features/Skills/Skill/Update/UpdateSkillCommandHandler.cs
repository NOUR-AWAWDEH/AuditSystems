using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Models.Skills;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Skills.Skill.Update;

internal sealed class UpdateSkillCommandHandler(
    ISkillService skillService,
    IMapper mapper,
    ILogger<UpdateSkillCommandHandler> logger)
    : IRequestHandler<UpdateSkillCommand, Result>
{
    public async Task<Result> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
    {
        var skillModel = mapper.Map<SkillModel>(request);
        await skillService.UpdateSkillAsync(skillModel);

        logger.LogInformation("Skill with ID {Id} updated successfully.", request.Id);
        return Result.Success();
    }
}
