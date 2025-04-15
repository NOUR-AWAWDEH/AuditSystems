using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Delete;

internal sealed class DeleteSkillCategoryCommandHandler(
    ISkillCategoryService skillCategoryService,
    ILogger<DeleteSkillCategoryCommandHandler> logger) :
    IRequestHandler<DeleteSkillCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteSkillCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await skillCategoryService.DeleteSkillCategoryAsync(request.Id);
            logger.LogInformation("Skill category with id: {Id} was deleted", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting skill category with id: {Id}", request.Id);
            return Result.Error(ex.Message);  
        }  
    }
}
