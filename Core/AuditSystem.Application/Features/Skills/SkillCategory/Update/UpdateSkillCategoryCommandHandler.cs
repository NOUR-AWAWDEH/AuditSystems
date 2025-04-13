using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Models.Skills;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Update;

internal sealed class UpdateSkillCategoryCommandHandler(
    ISkillCategoryService skillCategoryService,
    IMapper mapper,
    ILogger<UpdateSkillCategoryCommandHandler> logger)
    : IRequestHandler<UpdateSkillCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateSkillCategoryCommand request, CancellationToken cancellationToken)
    {
        var skillCategoryModel = mapper.Map<SkillCategoryModel>(request);
        await skillCategoryService.UpdateSkillCategoryAsync(skillCategoryModel);
        
        logger.LogInformation("Skill category with ID {Id} updated successfully.", request.Id);
        return Result.Success();
    }
}
