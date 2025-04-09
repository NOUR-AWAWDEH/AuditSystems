using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Models.Skills;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Create;

internal sealed class CreateSkillCategoryCommandHandler(
     ISkillCategoryService skillCategoryService,
    IMapper mapper,
    ILogger<CreateSkillCategoryCommandHandler> logger)
    : IRequestHandler<CreateSkillCategoryCommand, Result<CreateSkillCategoryCommandResponse>>
{
    public async Task<Result<CreateSkillCategoryCommandResponse>> Handle(CreateSkillCategoryCommand request, CancellationToken cancellationToken)
    {
        var skillCategoryModel = mapper.Map<SkillCategoryModel>(request);
        var skillCategoryId = await skillCategoryService.CreateSkillCategoryAsync(skillCategoryModel);
        logger.LogInformation("Skill with Name {SkillName} has been created.", request.Name);

        return Result<CreateSkillCategoryCommandResponse>.Created(new CreateSkillCategoryCommandResponse(skillCategoryId));
    }
}
