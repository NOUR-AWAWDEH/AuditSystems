using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Models.Skills;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Skills.SkillSet.Update;

internal sealed class UpdateSkillSetCommandHandler(
    ISkillSetService skillSetService,
    IMapper mapper,
    ILogger<UpdateSkillSetCommandHandler> logger)
    : IRequestHandler<UpdateSkillSetCommand, Result>
{
    public async Task<Result> Handle(UpdateSkillSetCommand request, CancellationToken cancellationToken)
    {
        var skillSetModel = mapper.Map<SkillSetModel>(request);
        await skillSetService.UpdateSkillSetAsync(skillSetModel);

        logger.LogInformation("Skill set with ID {Id} updated successfully.", request.Id);
        return Result.Success();
    }
}
