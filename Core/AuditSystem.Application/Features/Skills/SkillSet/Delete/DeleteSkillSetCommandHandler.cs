using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Skills.SkillSet.Delete;

internal class DeleteSkillSetCommandHandler(
    ISkillSetService skillSetService,
    ILogger<DeleteSkillSetCommandHandler> logger) :
    IRequestHandler<DeleteSkillSetCommand, Result>
{
    public async Task<Result> Handle(DeleteSkillSetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await skillSetService.DeleteSkillSetAsync(request.Id);  
            logger.LogInformation("Skill set with id: {Id} was deleted", request.Id);
            return Result.Success();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting skill set with id: {Id}", request.Id);
            return Result.Error(ex.Message);
        }      
    }
}